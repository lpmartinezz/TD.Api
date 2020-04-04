using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TD.WebApi.Entities;
using TD.WebApi.Logic;
using TD.WebApi.Models;

namespace TD.WebApi.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FormulariosController : ControllerBase
    {
        private readonly FEV2Context _context;

        public FormulariosController(FEV2Context context)
        {
            _context = context;
        }

        #region "METODOS PERSONALIZADOS"

        /// <summary>
        /// Autentificacion de Usuario
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna los datos de la autentificacion</returns>
        [EnableCors]
        [HttpPost("Autentificacion")]
        [ProducesResponseType(typeof(BaseResponse<ResultAuthentication>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultAuthentication>> Autentificacion(AutentificacionRequest request)
        {
            BaseResponse<ResultAuthentication> baseResponse = new BaseResponse<ResultAuthentication>
            {
                result = new ResultAuthentication()
            };
            long id_usuario = 0;
            try
            {
                //USUARIO
                baseResponse.result.usuario = (from u in _context.Usuario.Where(x => x.Login.Equals(request.CodigoUsuario))
                                               join c in _context.CanalComunicacion on u.Idusuario equals c.Idusuario into UsuCanAte
                                               from uca in UsuCanAte.DefaultIfEmpty()
                                               select new Entities.Usuario
                                               {
                                                   usuarioId = u.Idusuario,
                                                   rolId = 0,
                                                   codigoUsuario = u.Login,
                                                   claveSecreta = u.Password,
                                                   email = uca.Valor,
                                                   apellidoPaterno = u.Apellidos,
                                                   apellidoMaterno = u.Apellidos,
                                                   primerNombre = u.Nombres,
                                                   segundoNombre = u.Nombres,
                                                   alias = u.Nombres,
                                                   estado = metodos.RetornarInt(Convert.ToBoolean(u.Activo)),
                                                   empresaId = Convert.ToInt32(u.Idempresa)
                                               }).FirstOrDefault();
                if (baseResponse.result.usuario != null)
                {
                    id_usuario = baseResponse.result.usuario.usuarioId;
                    //MENU
                    baseResponse.result.menu = MenusByCodUsuario(baseResponse.result.usuario.empresaId);

                    //VALIDACION
                    baseResponse.result.validaUsuario = new Entities.ValidaUsuario
                    {
                        identificador = 1,
                        mensaje = null,
                        codigoUsuario = baseResponse.result.usuario.codigoUsuario,
                        isAutenticado = false
                    };

                    //Buscar sessiones para desactivar
                    var Usessiones = await _context.UserSesion.Where(x => x.Idusuario.Equals(id_usuario) && x.Activo.Equals(true)).ToListAsync();
                    foreach (var item in Usessiones)
                    {
                        Models.UserSesion usuario1 = await _context.UserSesion.FindAsync(item.Idsesion);
                        usuario1.Activo = false;
                        _context.UserSesion.Update(usuario1);
                        await _context.SaveChangesAsync();
                    }

                    //insert session nueva activa
                    UserSesion USinsert = new UserSesion
                    {
                        Idusuario = id_usuario, //baseResponse.result.usuario.usuarioId,
                        HoraFin = DateTime.Now,
                        EsNormalLogout = false,
                        Usuario = id_usuario, //baseResponse.result.usuario.usuarioId,
                        Activo = true,
                        Registro = DateTime.Now
                    };
                    _context.UserSesion.Add(USinsert);
                    await _context.SaveChangesAsync();

                    baseResponse.success = true;
                    baseResponse.code = "00000";
                    baseResponse.mensaje = "Usuario Autenticado Correctamente";
                }
                else
                {
                    baseResponse.result.validaUsuario = new Entities.ValidaUsuario
                    {
                        identificador = 0,
                        mensaje = null,
                        codigoUsuario = null,
                        isAutenticado = true
                    };

                    baseResponse.success = false;
                    baseResponse.code = "-1";
                    baseResponse.mensaje = "Metodo Autenticado No se encontró el Usuario.";
                }
            }
            catch (Exception ex)
            {
                baseResponse.result.validaUsuario = new Entities.ValidaUsuario
                {
                    identificador = 0,
                    mensaje = null,
                    codigoUsuario = null,
                    isAutenticado = true
                };

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo Autenticado Error:" + ex.Message;

                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(id_usuario) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo Autenticado Error:" + ex.Message,
                    UserTableName = "Autentificacion",
                    ValuePk = 0,
                    Usuario = id_usuario,
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();
            }
            return Ok(baseResponse);
        }

        /// <summary>
        /// Obtener todos los formularios
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna los datos de todos los formularios</returns>
        [HttpPost("GetAllFormularios")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ResultForm>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ResultForm>>>> GetAllFormularios(RequestForm request)
        {
            BaseResponse<IEnumerable<ResultForm>> baseResponse = new BaseResponse<IEnumerable<ResultForm>>();
            try
            {
                baseResponse.result = from F in _context.Formulario
                                      where F.Idempresa.Equals(Convert.ToInt64(request.idEmpresa))
                                      && F.Idusuario.Equals(Convert.ToInt64(request.idUsuario))
                                      select new ResultForm
                                      {
                                          idform = F.Idformulario.ToString(),
                                          titulo = F.Titulo,
                                          fecha_vigencia = F.FechaVigencia.ToShortDateString(),
                                          comentario = F.Descripcion,
                                          estado = metodos.RetornarString(Convert.ToBoolean(F.Activo))
                                      };
                if (baseResponse.result != null)
                {
                    baseResponse.success = true;
                    baseResponse.code = "0000";
                    baseResponse.mensaje = "Metodo GetAllFormularios";
                }
                else
                {
                    baseResponse.success = false;
                    baseResponse.code = "-1";
                    baseResponse.mensaje = "No se encontró registros";
                }
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo GetAllFormularios Error: " + ex.Message,
                    UserTableName = "GetAllFormularios",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Error al Cargar Formularios:" + ex.Message;
            }
            return Ok(baseResponse);
        }

        /// <summary>
        /// Obtener infor del formulario
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información del formulario</returns>
        [HttpPost("GetOneForm")]
        [ProducesResponseType(typeof(BaseResponse<ResultForm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ResultForm>>> GetOneForm(RequestOneForm request)
        {
            BaseResponse<ResultForm> baseResponse = new BaseResponse<ResultForm>
            {
                result = new ResultForm()
            };
            try
            {
                var formulario = _context.Formulario.FirstOrDefault(x => x.Idempresa.Equals(Convert.ToInt64(request.idEmpresa)) &&
                                                                          x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) &&
                                                                          x.Idformulario.Equals(Convert.ToInt64(request.idForm))
                                                                          );
                if (formulario != null)
                {
                    baseResponse.result.idform = formulario.Idformulario.ToString();
                    baseResponse.result.titulo = formulario.Titulo;
                    baseResponse.result.comentario = formulario.Descripcion;
                    baseResponse.result.fecha_vigencia = formulario.FechaVigencia.ToShortDateString();
                    baseResponse.result.idusuario = formulario.Idusuario.ToString();
                    baseResponse.result.idempresa = formulario.Idempresa.ToString();
                    baseResponse.result.controls = await ControlsByFormId(formulario.Idformulario);

                    baseResponse.success = true;
                    baseResponse.code = "0000";
                    baseResponse.mensaje = "Metodo GetOneFormularios";
                }
                else
                {
                    baseResponse.success = false;
                    baseResponse.code = "-1";
                    baseResponse.mensaje = "No se encontró registros";
                }
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo GetOneFormularios Error:" + ex.Message,
                    UserTableName = "GetAllFormularios",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Error al Cargar Formularios:" + ex.Message;
            }
            return Ok(baseResponse);
        }

        /// <summary>
        /// Insertar formulario
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información de la insercion del formulario</returns>
        [HttpPost("InsertForm")]
        [ProducesResponseType(typeof(BaseResponse<ResultIU>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ResultIU>>> InsertForm(ResultForm request)
        {
            BaseResponse<ResultIU> baseResponse = new BaseResponse<ResultIU>
            {
                result = new ResultIU()
            };
            try
            {
                Formulario forinsert = new Formulario
                {
                    Titulo = request.titulo,
                    Descripcion = request.comentario,
                    FechaVigencia = Convert.ToDateTime(request.fecha_vigencia),
                    Idusuario = Convert.ToInt64(request.idusuario),
                    Idempresa = Convert.ToInt64(request.idempresa),
                    Usuario = Convert.ToInt64(request.idusuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                //Insertar Registro en Formulario
                _context.Formulario.Add(forinsert);
                await _context.SaveChangesAsync();

                if (forinsert.Idformulario > 0)
                {
                    int orden = 0;
                    //insertar controles
                    foreach (var item in request.controls)
                    {
                        var TipoControl = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoControl") && x.Valor.Equals(item.tipo.ToString()));
                        var TipoRespuesta = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoRespuesta") && x.Valor.Equals(item.tipo_respuesta.ToString()));
                        var TipoSimbolo = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoSimbolo") && x.Valor.Equals(item.tipo_simbolo));
                        var Nivel = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("Nivel") && x.Valor.Equals(item.niveles));
                        var TipoRestriccion = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoRestriccion") && x.Valor.Equals(item.restriccion));

                        Models.Control coninsert = new Models.Control
                        {
                            Htmlid = item.id,
                            Idformulario = forinsert.Idformulario,
                            IdtipoControl = TipoControl.Idconfiguracion,
                            Texto = item.pregunta,
                            Descripcion = string.Empty,
                            Opciones = metodos.LeerOpciones(item.opciones),
                            IdtipoRespuesta = TipoRespuesta.Idconfiguracion,
                            Htmlname = item.controlid
                        };
                        if (item.respuesta_obligatoria == "1")
                        {
                            coninsert.EsRequerido = true;
                        }
                        else
                        {
                            coninsert.EsRequerido = false;
                        }
                        if (item.respuesta_larga == "1")
                        {
                            coninsert.EsRespuestaLarga = true;
                        }
                        else
                        {
                            coninsert.EsRespuestaLarga = false;
                        }
                        if (TipoSimbolo != null)
                        {
                            coninsert.IdtipoSimbolo = TipoSimbolo.Idconfiguracion;
                        }
                        else
                        {
                            coninsert.IdtipoSimbolo = null;
                        }
                        if (Nivel != null)
                        {
                            coninsert.Idnivel = Nivel.Idconfiguracion;
                        }
                        else
                        {
                            coninsert.Idnivel = null;
                        }
                        if (TipoRestriccion != null)
                        {
                            coninsert.IdtipoRestriccion = TipoRestriccion.Idconfiguracion;
                        }
                        else
                        {
                            coninsert.IdtipoRestriccion = null;
                        }
                        coninsert.IntervaloInicio = item.valor1;
                        coninsert.IntervaloFin = item.valor2;
                        coninsert.Orden = orden + 1;
                        coninsert.Usuario = Convert.ToInt64(request.idusuario);
                        coninsert.Activo = true;
                        coninsert.Registro = DateTime.Now;
                        _context.Control.Add(coninsert);
                        await _context.SaveChangesAsync();

                        baseResponse.result.idForm = Convert.ToInt32(forinsert.Idformulario);
                        baseResponse.result.mensaje = "Se insertó correctamente";

                        baseResponse.success = true;
                        baseResponse.code = "00000";
                        baseResponse.mensaje = "Metodo InsertForm";
                    }
                }
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idusuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo InsertForm Error:" + ex.Message,
                    UserTableName = "InsertForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idusuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.result.idForm = 0;
                baseResponse.result.mensaje = "No se pudo registrar el Formulario. " + ex.ToString();

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo InsertForm Error";
            }
            return Ok(baseResponse);
        }

        /// <summary>
        /// Actualizar formulario
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información de la actualizacion del formulario</returns>
        [HttpPost("UpdateFormulario")]
        [ProducesResponseType(typeof(BaseResponse<ResultIU>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ResultIU>>> UpdateForm(ResultForm request)
        {
            //int formularioversion;

            BaseResponse<ResultIU> baseResponse = new BaseResponse<ResultIU>
            {
                result = new ResultIU()
            };
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //ANULAR LOS CONTROLES DEL FORMULARIO
                        IEnumerable<Models.Control> lstControlAnular = GetControls(Convert.ToInt64(request.idform));
                        foreach (var itemA in lstControlAnular)
                        {
                            itemA.Activo = false;
                            _context.Control.Update(itemA);
                            await _context.SaveChangesAsync();
                        }

                        //Actualizar Registro en Formulario
                        Formulario forupdate = new Formulario
                        {
                            Idformulario = Convert.ToInt64(request.idform),
                            Titulo = request.titulo,
                            Descripcion = request.comentario,
                            FechaVigencia = Convert.ToDateTime(request.fecha_vigencia),
                            Idusuario = Convert.ToInt64(request.idusuario),
                            Idempresa = Convert.ToInt64(request.idempresa),
                            Usuario = Convert.ToInt64(request.idusuario),
                            //Version = Version+1,
                            //formularioversion = Version,
                            Activo = true,
                            Registro = DateTime.Now
                        };

                        _context.Formulario.Update(forupdate);
                        await _context.SaveChangesAsync();

                        if (forupdate.Idformulario > 0)
                        {
                            int orden = 0;
                            //insertar controles
                            foreach (var item in request.controls)
                            {
                                Models.Control conupdate = new Models.Control
                                {
                                    Idformulario = forupdate.Idformulario,
                                    Htmlid = item.id, // string.Empty,
                                    Htmlname = item.controlid,
                                    IdtipoControl = ValorTipoControlId(item.tipo),
                                    Texto = item.pregunta,
                                    Descripcion = string.Empty,
                                    Opciones = metodos.LeerOpciones(item.opciones),
                                    IdtipoRespuesta = ValorTipoRespuestaId(item.pregunta),
                                    EsRequerido = Convert.ToBoolean(metodos.RetornarBoolean(item.respuesta_obligatoria)),
                                    EsRespuestaLarga = metodos.RetornarBoolean(item.respuesta_larga),
                                    IdtipoSimbolo = ValorTipoSimboloId(item.tipo_simbolo),
                                    Idnivel = ValorNivelId(item.niveles),
                                    IdtipoRestriccion = ValorTipoRestriccionId(item.restriccion),
                                    IntervaloInicio = item.valor1,
                                    IntervaloFin = item.valor2,
                                    Orden = orden + 1,
                                    //FormularioVersion = formularioversion,
                                    Usuario = Convert.ToInt64(request.idusuario),
                                    Activo = true,
                                    Registro = DateTime.Now
                                };
                                _context.Control.Add(conupdate);
                                await _context.SaveChangesAsync().ConfigureAwait(false);
                                baseResponse.result.idForm = Convert.ToInt32(forupdate.Idformulario);
                                baseResponse.result.mensaje = "Se modificó correctamente";

                                baseResponse.success = true;
                                baseResponse.code = "00000";
                                baseResponse.mensaje = "Metodo UpudateForm";
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idusuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo UpudateForm Error:" + ex.Message,
                    UserTableName = "UpudateForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idusuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.result.idForm = 0;
                baseResponse.result.mensaje = "No se pudo actualizar el Formulario. " + ex.ToString();
                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo UpudateForm Error";
            }
            return Ok(baseResponse);
        }

        /// <summary>
        /// Eliminar formulario
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información de la eliminacion del formulario</returns>
        [HttpPost("DeleteFormulario")]
        [ProducesResponseType(typeof(BaseResponse<int?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<int?>>> DeleteForm(RequestOneForm request)
        {
            BaseResponse<int?> baseResponse = new BaseResponse<int?>();
            try
            {
                //Eliminar Controles
                var controles = await _context.Control.Where(x => x.Idformulario.Equals(Convert.ToInt64(request.idForm))).ToListAsync();

                if (controles == null)
                {
                    return NotFound();
                }
                else
                {
                    foreach (var item in controles)
                    {
                        _context.Control.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                }
                //Eliminar Forms
                //Valores a eliminar
                long lform = Convert.ToInt64(request.idForm);
                long lempresa = Convert.ToInt64(request.idEmpresa);
                long lusuario = Convert.ToInt64(request.idUsuario);
                var formulario = _context.Formulario.FirstOrDefault(x => x.Idformulario.Equals(lform) & x.Idempresa.Equals(lempresa) && x.Idusuario.Equals(lusuario));
                if (formulario == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Formulario.Remove(formulario);
                    await _context.SaveChangesAsync();
                }
                baseResponse.success = true;
                baseResponse.code = "00000";
                baseResponse.mensaje = "Metodo DeleteFormulario";
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo UpudateForm Error:" + ex.Message,
                    UserTableName = "UpudateForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo DeleteFormulario Error. " + ex.ToString();
            }
            baseResponse.result = null;
            return Ok(baseResponse);
        }

        #endregion

        #region "Enviar Base de Datos"

        /// <summary>
        /// Obtener todos las Base de Datos
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna los datos de todas las Base de Datos</returns>
        [HttpPost("GetAllBaseDatos")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ResponseBaseDatos>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ResponseBaseDatos>>>> GetAllBaseDatos(RequestForm request)
        {
            BaseResponse<IEnumerable<ResponseBaseDatos>> resultado = new BaseResponse<IEnumerable<ResponseBaseDatos>>();
            try
            {
                resultado.result = from F in _context.UserDbdefinition
                                   where F.Idempresa.Equals(Convert.ToInt64(request.idEmpresa))
                                   && F.Idusuario.Equals(Convert.ToInt64(request.idUsuario))
                                   select new ResponseBaseDatos
                                   {
                                       idbd = F.IduserDb.ToString(),
                                       name = F.Descripcion,
                                       idusuario = F.Idusuario.ToString(),
                                       idempresa = F.Idempresa.ToString(),
                                       fecha_upload = F.Registro.ToShortDateString() + " " + F.Registro.ToLongTimeString(),
                                       estado = metodos.RetornarInt(Convert.ToBoolean(F.Activo))
                                   };
                if (resultado.result != null)
                {
                    resultado.success = true;
                    resultado.code = "0000";
                    resultado.mensaje = "Metodo GetAllBaseDatos";
                }
                else
                {
                    resultado.success = false;
                    resultado.code = "-1";
                    resultado.mensaje = "No se encontró registros";
                }
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo UpudateForm Error:" + ex.Message,
                    UserTableName = "UpudateForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                resultado.success = false;
                resultado.code = "-1";
                resultado.mensaje = "Error al Cargar GetAllBaseDatos:" + ex.Message;
            }
            return Ok(resultado);
        }

        /// <summary>
        /// Obtener infor de la Base de Datos
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información de la Base de datos</returns>
        [HttpPost("GetOneBaseDatos")]
        [ProducesResponseType(typeof(BaseResponse<ResultBaseDatoOne>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ResultBaseDatoOne>>> GetOneBaseDatos(RequestBaseDatosOne request)
        {
            string sHeader = string.Empty;
            string sRows = string.Empty;

            BaseResponse<ResultBaseDatoOne> baseResponse = new BaseResponse<ResultBaseDatoOne>
            {
                result = new ResultBaseDatoOne()
            };
            try
            {
                sHeader = (from F in _context.UserDbdefinition
                           where F.Idempresa.Equals(Convert.ToInt64(request.idEmpresa))
                               && F.Idusuario.Equals(Convert.ToInt64(request.idUsuario))
                               && F.IduserDb.Equals(Convert.ToInt64(request.idbd))
                           select new
                           {
                               header = F.Header
                           }).FirstOrDefault().header;

                string jsonStringH = sHeader;

                baseResponse.result.Header = JsonConvert.DeserializeObject<IEnumerable<HeaderRequest>>(jsonStringH);

                sRows = (from F in _context.UserDbrows
                         where F.IduserDb.Equals(Convert.ToInt64(request.idbd))
                         select new
                         {
                             rows = F.UserDataRow
                         }).FirstOrDefault().rows;

                string jsonStringR = sRows;
                //baseResponse.result.rows = JsonConvert.DeserializeObject<IEnumerable<RowRequest>>(jsonStringR);

                baseResponse.result.rows = JsonConvert.DeserializeObject<IEnumerable<object>>(jsonStringR);

                baseResponse.success = true;
                baseResponse.code = "0000";
                baseResponse.mensaje = "Metodo GetOneFormularios";
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo UpudateForm Error:" + ex.Message,
                    UserTableName = "UpudateForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Error al Cargar Formularios:" + ex.Message;
            }
            return Ok(baseResponse);
        }

        /// <summary>
        /// Enviar Base de Datos
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información dl envio de la base de datos</returns>
        /// 
        [HttpPost("SendBaseDatos")]
        [ProducesResponseType(typeof(BaseResponse<SaveFormResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<SaveFormResponse>>> SendBaseDatos(SendBaseDatoRequest request)
        {
            BaseResponse<SaveFormResponse> baseResponse = new BaseResponse<SaveFormResponse>
            {
                result = new SaveFormResponse()
            };
            try
            {
                string sNombreBD = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                                   DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                
                //insertar Headers
                string SetJsonH = JsonConvert.SerializeObject(request.Header);

                UserDbdefinition Headerinsert = new UserDbdefinition
                {
                    Idempresa = Convert.ToInt64(request.idEmpresa),
                    Idusuario = Convert.ToInt64(request.idUsuario),
                    Descripcion = "bd_" + sNombreBD,
                    Header = SetJsonH,
                    EsPublicada = false,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserDbdefinition.Add(Headerinsert);
                await _context.SaveChangesAsync();

                string SetJsonR = string.Empty;
                //insertar Rows
                SetJsonR = JsonConvert.SerializeObject(request.rows);
                UserDbrows Rowinsert = new UserDbrows
                {
                    IduserDb = Headerinsert.IduserDb,
                    UserDataRow = SetJsonR,
                    Usuario = 1,
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserDbrows.Add(Rowinsert);
                await _context.SaveChangesAsync();

                baseResponse.result.data = "0";
                baseResponse.result.mensaje = "Se cargó correctamente";

                baseResponse.success = true;
                baseResponse.code = "00000";
                baseResponse.mensaje = "Metodo SendBaseDatos";
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo UpudateForm Error:" + ex.Message,
                    UserTableName = "UpudateForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.result.data = "-1";
                baseResponse.result.mensaje = "Error en la carga de la Base";

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo SendBaseDatos Error. " + ex.ToString();
            }
            return Ok(baseResponse);
        }

        #endregion

        #region "Enviar Invitacion"

        /// <summary>
        /// Enviar Invitación
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información del envio de las invitaciones</returns>
        [HttpPost("SendInvitation")]
        [ProducesResponseType(typeof(BaseResponse<ResultProcess>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ResultProcess>>> SendInvitation(SendInvitationRequest request)
        {
            BaseResponse<SaveFormResponse> baseResponse = new BaseResponse<SaveFormResponse>
            {
                result = new SaveFormResponse()
            };
            try
            {
                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo SendInvitation Error.";

                //valida que existe el formulario
                var formulario = _context.Formulario.Where(x => x.Idformulario.Equals(Convert.ToInt64(request.idForm)) && x.Activo.Equals(true)).FirstOrDefault();
                if (formulario == null)
                {
                    baseResponse.result.data = "-1";
                    baseResponse.result.mensaje = "No existe el formulario";
                }
                else
                {
                    //valida que el formulario esté vigente
                    if (formulario.FechaVigencia < DateTime.Now)
                    {
                        baseResponse.result.data = "-1";
                        baseResponse.result.mensaje = "El formulario esta fuera de fecha de vigencia";
                    }
                    else
                    {
                        //validar que exista la Base de datos
                        var DataBaseUser = _context.UserDbdefinition.Where(x => x.IduserDb.Equals(Convert.ToInt64(request.idBaseDatos)) && x.Activo.Equals(true)).FirstOrDefault();
                        if (DataBaseUser == null)
                        {
                            baseResponse.result.data = "-1";
                            baseResponse.result.mensaje = "La Base de datos no existe";
                        }
                        else
                        {
                            UrlQS urlQS = new UrlQS();
                            string sHeader = string.Empty;
                            string sRows = string.Empty;

                            sHeader = (from F in _context.UserDbdefinition
                                       where F.IduserDb.Equals(Convert.ToInt64(request.idBaseDatos))
                                       select new
                                       {
                                           header = F.Header
                                       }).FirstOrDefault().header;
                            string jsonStringH = sHeader;
                            IEnumerable<HeaderRequest> headerRequests = JsonConvert.DeserializeObject<IEnumerable<HeaderRequest>>(jsonStringH);

                            

                            sRows = (from F in _context.UserDbrows
                                     where F.IduserDb.Equals(Convert.ToInt64(request.idBaseDatos))
                                     select new
                                     {
                                         rows = F.UserDataRow
                                     }).FirstOrDefault().rows;

                            string jsonStringR = sRows;
                            //List<RowRequest> rowRequests = JsonConvert.DeserializeObject<List<RowRequest>>(jsonStringR);
                            IEnumerable<RowRequest> rowRequests = JsonConvert.DeserializeObject<IEnumerable<RowRequest>>(jsonStringR);

                            //Guardar FormInvitation
                            FormInvitation formInvitation = new FormInvitation
                            {
                                Idformulario = Convert.ToInt64(request.idForm),
                                IduserDb = Convert.ToInt64(request.idBaseDatos),
                                Quniverse = rowRequests.Count(),
                                QsendInvitation = 0,
                                Qresponse = 0,
                                Qleft = 0,
                                IsOnLine = false,
                                Usuario = Convert.ToInt64(request.idUsuario),
                                Activo = true,
                                Registro = DateTime.Now
                            };
                            _context.FormInvitation.Add(formInvitation);
                            await _context.SaveChangesAsync();

                            //rowRequests
                            int indexH = 0;
                            foreach (var itemR in rowRequests)
                            {
                                FormInvitationDetail formInvitationDetail = new FormInvitationDetail();
                                formInvitationDetail.IdformInvition = formInvitation.IdformInvition;
                                formInvitationDetail.IsAnswered = false;
                                foreach (var itemH in headerRequests) 
                                {
                                    int colrow = 0;
                                    if (itemH.typeColumn != "0")
                                    {
                                        foreach (var prop in itemR.GetType().GetProperties())
                                        {
                                            if (indexH == colrow)
                                            {
                                                var ValorAtributo = prop.GetValue(itemR, null);
                                                if (ValorAtributo != null)
                                                {
                                                    switch (itemH.typeColumn)
                                                    {
                                                        case "1":
                                                            formInvitationDetail.Name = ValorAtributo.ToString();
                                                            break;
                                                        case "2":
                                                            formInvitationDetail.Mail = ValorAtributo.ToString();
                                                            break;
                                                        case "3":
                                                            formInvitationDetail.Idcard = ValorAtributo.ToString();
                                                            break;
                                                        case "4":
                                                            formInvitationDetail.Mobile = ValorAtributo.ToString();
                                                            break;
                                                    }
                                                }
                                                //break;
                                            }
                                            colrow += 1;
                                            //break;
                                        }
                                    }
                                    indexH += 1;
                                }
                                formInvitationDetail.Urlqs = urlQS.GenerarQS(formInvitationDetail.Name, "", formInvitationDetail.Mail, formInvitationDetail.Idcard, formInvitationDetail.Mobile);
                                formInvitationDetail.Usuario = Convert.ToInt64(request.idUsuario);
                                formInvitationDetail.Activo = true;
                                formInvitationDetail.Registro = DateTime.Now;
                                _context.FormInvitationDetail.Add(formInvitationDetail);
                                await _context.SaveChangesAsync();
                                indexH = 0;
                            }
                            baseResponse.result.data = "0";
                            baseResponse.result.mensaje = "Se envió correctamente";

                            baseResponse.success = true;
                            baseResponse.code = "00000";
                            baseResponse.mensaje = "Metodo SendInvitation";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idUsuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo UpudateForm Error:" + ex.Message,
                    UserTableName = "UpudateForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idUsuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.result.data = "-1";
                baseResponse.result.mensaje = "Error en envío de la invitación";

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo SendInvitation Error. " + ex.ToString();
            }
            return Ok(baseResponse);
        }

        #endregion

        #region "Guardar Cuestionario"

        /// <summary>
        /// Guardar Formulario
        /// </summary>
        /// <response code="404"></response>
        /// <returns>Retorna información de la insercion del formulario</returns>
        [HttpPost("SaveFormResponses")]
        [ProducesResponseType(typeof(BaseResponse<SaveFormResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<SaveFormResponse>>> SaveFormResponses(SaveFormRequest request)
        {
            BaseResponse<SaveFormResponse> baseResponse = new BaseResponse<SaveFormResponse>
            {
                result = new SaveFormResponse()
            };
            try
            {
                foreach (var item in request.controls)
                {
                    Models.Respuesta respuesta = new Models.Respuesta
                    {
                        Idformulario = Convert.ToInt64(request.idform),
                        Idcontrol = item.id,
                        Respuesta1 = item.value,
                        Usuario = Convert.ToInt64(request.idusuario),
                        Activo = true,
                        Registro = DateTime.Now
                    };
                    _context.Respuesta.Add(respuesta);
                    await _context.SaveChangesAsync();
                }
                baseResponse.result.data = "0";
                baseResponse.result.mensaje = "Se registro correctamente las respuesta.";

                baseResponse.success = true;
                baseResponse.code = "00000";
                baseResponse.mensaje = "Metodo SaveFormResponses";
            }
            catch (Exception ex)
            {
                ////insertar EventLog
                UserEventLog userEventLog = new UserEventLog
                {
                    IdeventType = 3, //Error
                    Idsesion = _context.UserSesion.Where(x => x.Idusuario.Equals(Convert.ToInt64(request.idusuario)) && x.Activo.Equals(true)).FirstOrDefault().Idsesion,
                    LastValue = string.Empty,
                    TextMessage = "Metodo UpudateForm Error:" + ex.Message,
                    UserTableName = "UpudateForm",
                    ValuePk = 0,
                    Usuario = Convert.ToInt64(request.idusuario),
                    Activo = true,
                    Registro = DateTime.Now
                };
                _context.UserEventLog.Add(userEventLog);
                await _context.SaveChangesAsync();

                baseResponse.result.data = "0";
                baseResponse.result.mensaje = "No se pudo registrar la respuesta." + ex.ToString();

                baseResponse.success = false;
                baseResponse.code = "-1";
                baseResponse.mensaje = "Metodo SaveFormResponses Error";
            }
            return Ok(baseResponse);
        }

        #endregion

        #region "Metodos"

        private readonly Metodos metodos = new Metodos();

        private IEnumerable<Models.Control> GetControls(long idForm)
        {
            return _context.Control.Where(x => x.Idformulario.Equals(idForm)).ToList();
        }

        private IEnumerable<Entities.Menu> MenusByCodUsuario(long idEmpresa)
        {
            var vMenus = _context.Menu.Where(x => x.Idempresa.Equals(idEmpresa)).ToList();
            var menus = from F in vMenus
                        select new Entities.Menu
                        {
                            menuId = F.Idmenu,
                            menuPadreId = F.Idparent,
                            codigoIdentificador = F.CodigoIdentificador,
                            descripcion = F.Descripcion,
                            area = F.Area,
                            controlador = F.Controlador,
                            accion = F.Accion,
                            icono = F.Icono,
                            orden = F.Orden,
                            estado = F.Activo
                        };
            return menus;
        }

        private async Task<IEnumerable<Entities.Control>> ControlsByFormId(long idform)
        {
            var Vcontrol = await _context.Control.Where(x => x.Idformulario.Equals(idform) && x.Activo.Equals(true)).ToListAsync();
            var control = from F in Vcontrol
                          select new Entities.Control
                          {
                              id = F.Htmlid,
                              tipo = TipoControlIdValor(F.IdtipoControl), //Convert.ToInt32(F.IdtipoControl),
                              pregunta = F.Texto,
                              opciones = metodos.CargaOpciones(F.Opciones),
                              tipo_respuesta = TipoRespuestaIdValor(F.IdtipoRespuesta),
                              respuesta_obligatoria = metodos.RetornarString(F.EsRequerido),
                              respuesta_larga = metodos.RetornarString(F.EsRespuestaLarga),
                              tipo_simbolo = TipoSimboloIdValor(F.IdtipoSimbolo),
                              niveles = NivelIdValor(F.Idnivel),
                              restriccion = TipoRestriccionIdValor(F.IdtipoRestriccion.ToString()),
                              valor1 = F.IntervaloInicio,
                              valor2 = F.IntervaloFin,
                              controlid = F.Htmlname
                          };
            return control;
        }

        private string TipoControlIdValor(long idTipoControl)
        {
            string id = string.Empty;
            try
            {
                var TipoControl = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoControl") && x.Idconfiguracion.Equals(idTipoControl));
                if (TipoControl != null)
                {
                    id = TipoControl.Valor;
                }
            }
            catch 
            {
                id = "0";
            }
            return id;
        }

        private string TipoRespuestaIdValor(long idTipoRespuesta)
        {
            string id = string.Empty;
            var TipoRespuesta = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoRespuesta") && x.Idconfiguracion.Equals(idTipoRespuesta));
            if (TipoRespuesta != null)
            {
                id = TipoRespuesta.Valor;
            }
            return id;
        }

        private string TipoSimboloIdValor(long? idTipoSimbolo)
        {
            string id = string.Empty;
            if (idTipoSimbolo != null)
            {
                var TipoSimbolo = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoSimbolo") && x.Idconfiguracion.Equals(idTipoSimbolo));
                if (TipoSimbolo != null)
                {
                    id = TipoSimbolo.Valor;
                }
            }
            return id;
        }

        private string NivelIdValor(long? idNivel)
        {
            string id = string.Empty;
            if (idNivel != null)
            {
                var Nivel = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("Nivel") && x.Idconfiguracion.Equals(idNivel));
                if (Nivel != null)
                {
                    id = Nivel.Valor;
                }
            }
            return id;
        }

        private string TipoRestriccionIdValor(string idTipoRestriccion)
        {
            string id = string.Empty;
            var TipoRestriccion = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoRestriccion") && x.Valor.Equals(idTipoRestriccion));
            if (TipoRestriccion != null)
            {
                id = TipoRestriccion.Valor;
            }
            return id;
        }

        private long ValorTipoControlId(string idTipoControl)
        {
            long id = 0;
            var TipoControl = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoControl") && x.Valor.Equals(idTipoControl));
            if (TipoControl != null)
            {
                id = TipoControl.Idconfiguracion;
            }
            return id;
        }

        private long ValorTipoRespuestaId(string idTipoRespuesta)
        {
            long id = 0;
            var TipoRespuesta = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoRespuesta") && x.Valor.Equals(idTipoRespuesta));
            if (TipoRespuesta != null)
            {
                id = TipoRespuesta.Idconfiguracion;
            }
            return id;
        }

        private long? ValorTipoSimboloId(string idTipoSimbolo)
        {
            long? id = null;
            if (!string.IsNullOrEmpty(idTipoSimbolo))
            {
                var TipoSimbolo = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoSimbolo") && x.Valor.Equals(idTipoSimbolo));
                if (TipoSimbolo != null)
                {
                    id = TipoSimbolo.Idconfiguracion;
                }
            }
            return id;
        }

        private long? ValorNivelId(string idNivel)
        {
            long? id = null;
            if (!string.IsNullOrEmpty(idNivel))
            {
                var Nivel = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("Nivel") && x.Valor.Equals(idNivel));
                if (Nivel != null)
                {
                    id = Nivel.Idconfiguracion;
                }
            }
            return id;
        }

        private long? ValorTipoRestriccionId(string idTipoRestriccion)
        {
            long? id = null;
            if (!string.IsNullOrEmpty(idTipoRestriccion))
            {
                var TipoRestriccion = _context.Configuracion.FirstOrDefault(x => x.Grupo.Equals("IDTipoRestriccion") && x.Valor.Equals(idTipoRestriccion));
                if (TipoRestriccion != null)
                {
                    id = TipoRestriccion.Idconfiguracion;
                }
            }
            return id;
        }

        #endregion
    }
}
