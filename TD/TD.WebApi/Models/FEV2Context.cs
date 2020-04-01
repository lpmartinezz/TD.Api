using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TD.WebApi.Models
{
    public partial class FEV2Context : DbContext
    {
        //public FEV2Context()
        //{
        //}

        public FEV2Context(DbContextOptions<FEV2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CanalComunicacion> CanalComunicacion { get; set; }
        public virtual DbSet<Configuracion> Configuracion { get; set; }
        public virtual DbSet<Control> Control { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<FormInvitation> FormInvitation { get; set; }
        public virtual DbSet<FormInvitationDetail> FormInvitationDetail { get; set; }
        public virtual DbSet<Formulario> Formulario { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Publicacion> Publicacion { get; set; }
        public virtual DbSet<Respuesta> Respuesta { get; set; }
        public virtual DbSet<RespuestaValoracion> RespuestaValoracion { get; set; }
        public virtual DbSet<UserDbdefinition> UserDbdefinition { get; set; }
        public virtual DbSet<UserDbrows> UserDbrows { get; set; }
        public virtual DbSet<UserEventLog> UserEventLog { get; set; }
        public virtual DbSet<UserSesion> UserSesion { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=(local);Database=FEV2;Integrated Security=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CanalComunicacion>(entity =>
            {
                entity.HasKey(e => e.Idcanal);

                entity.HasIndex(e => e.IdtipoCanal)
                    .HasName("IX_CCTipoCanal");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("IX_CCIDUsuario");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_CCRegistro");

                entity.HasIndex(e => e.Valor)
                    .HasName("IX_CCValor");

                entity.Property(e => e.Idcanal).HasColumnName("IDCanal");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdtipoCanal).HasColumnName("IDTipoCanal");

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.Idconfiguracion);

                entity.HasIndex(e => e.Clave)
                    .HasName("IX_ConfigClave");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_ConfigRegistro");

                entity.HasIndex(e => e.Valor)
                    .HasName("IX_ConfigValor");

                entity.Property(e => e.Idconfiguracion).HasColumnName("IDConfiguracion");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Grupo)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Control>(entity =>
            {
                entity.HasKey(e => e.Idcontrol);

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_CActivo");

                entity.HasIndex(e => e.Idformulario)
                    .HasName("IX_CTFormulario");

                entity.HasIndex(e => e.IdtipoControl)
                    .HasName("IX_CTTipoControl");

                entity.HasIndex(e => e.IdtipoRespuesta)
                    .HasName("IX_CTTipoRespuesta");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_CTRegistro");

                entity.Property(e => e.Idcontrol).HasColumnName("IDControl");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EsRespuestaLarga).HasDefaultValueSql("((0))");

                entity.Property(e => e.Htmlid)
                    .IsRequired()
                    .HasColumnName("HTMLID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Htmlname)
                    .IsRequired()
                    .HasColumnName("HTMLName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idformulario).HasColumnName("IDFormulario");

                entity.Property(e => e.Idnivel).HasColumnName("IDNivel");

                entity.Property(e => e.IdtipoControl).HasColumnName("IDTipoControl");

                entity.Property(e => e.IdtipoRespuesta).HasColumnName("IDTipoRespuesta");

                entity.Property(e => e.IdtipoRestriccion).HasColumnName("IDTipoRestriccion");

                entity.Property(e => e.IdtipoSimbolo).HasColumnName("IDTipoSimbolo");

                entity.Property(e => e.IntervaloFin)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.IntervaloInicio)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Opciones)
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.Orden).HasDefaultValueSql("((0))");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Texto)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.Idempresa);

                entity.HasIndex(e => e.Idfiscal)
                    .HasName("IX_EIDFiscal");

                entity.HasIndex(e => e.Idpais)
                    .HasName("IX_EPais");

                entity.HasIndex(e => e.Idresponsable)
                    .HasName("IX_EResponsable");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_ERegistro");

                entity.Property(e => e.Idempresa).HasColumnName("IDEmpresa");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Idfiscal)
                    .IsRequired()
                    .HasColumnName("IDFiscal")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Idpais)
                    .HasColumnName("IDPais")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Idresponsable).HasColumnName("IDResponsable");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<FormInvitation>(entity =>
            {
                entity.HasKey(e => e.IdformInvition);

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_FIActivo");

                entity.HasIndex(e => e.Idformulario)
                    .HasName("IX_FIIDFormulario");

                entity.HasIndex(e => e.IduserDb)
                    .HasName("IX_FIIDUserDB");

                entity.HasIndex(e => e.IsOnLine)
                    .HasName("IX_FIIsOnline");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_FIRegistro");

                entity.Property(e => e.IdformInvition).HasColumnName("IDFormInvition");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Idformulario).HasColumnName("IDFormulario");

                entity.Property(e => e.IduserDb).HasColumnName("IDUserDB");

                entity.Property(e => e.IsOnLine)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Qleft).HasColumnName("QLeft");

                entity.Property(e => e.Qresponse).HasColumnName("QResponse");

                entity.Property(e => e.QsendInvitation).HasColumnName("QSendInvitation");

                entity.Property(e => e.Quniverse).HasColumnName("QUniverse");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<FormInvitationDetail>(entity =>
            {
                entity.HasKey(e => e.Idfidetail);

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_FIDActivo");

                entity.HasIndex(e => e.Idcard)
                    .HasName("IX_FIDIDCard");

                entity.HasIndex(e => e.IdformInvition)
                    .HasName("IX_FIDIDFormInvition");

                entity.HasIndex(e => e.IsAnswered)
                    .HasName("IX_FIDIsAnswered");

                entity.HasIndex(e => e.Mail)
                    .HasName("IX_FIDMail");

                entity.HasIndex(e => e.Mobile)
                    .HasName("IX_FIDMobile");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_FIDRegistro");

                entity.HasIndex(e => e.Urlqs)
                    .HasName("IX_FIDURLQS");

                entity.Property(e => e.Idfidetail).HasColumnName("IDFIDetail");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcard)
                    .IsRequired()
                    .HasColumnName("IDCard")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdformInvition).HasColumnName("IDFormInvition");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Urlqs)
                    .IsRequired()
                    .HasColumnName("URLQS")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Formulario>(entity =>
            {
                entity.HasKey(e => e.Idformulario);

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_FActivo");

                entity.HasIndex(e => e.Idempresa)
                    .HasName("IX_FEmpresa");

                entity.HasIndex(e => e.IdtipoFormulario)
                    .HasName("IX_FTipoFormulario");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("IX_FUsuario");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_FRegistro");

                entity.HasIndex(e => e.Titulo)
                    .HasName("IX_FTitulo");

                entity.Property(e => e.Idformulario).HasColumnName("IDFormulario");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaVigencia).HasColumnType("datetime");

                entity.Property(e => e.Idempresa).HasColumnName("IDEmpresa");

                entity.Property(e => e.IdtipoFormulario)
                    .HasColumnName("IDTipoFormulario")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Idmenu);

                entity.HasIndex(e => e.Idempresa)
                    .HasName("IX_MIDEmpresa");

                entity.HasIndex(e => e.Idparent)
                    .HasName("IX_MParent");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_MRegistro");

                entity.Property(e => e.Idmenu).HasColumnName("IDMenu");

                entity.Property(e => e.Accion)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Area)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoIdentificador)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Controlador)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Icono)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Idempresa).HasColumnName("IDEmpresa");

                entity.Property(e => e.Idparent).HasColumnName("IDParent");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Publicacion>(entity =>
            {
                entity.HasKey(e => e.Idpublicacion);

                entity.HasIndex(e => e.FechaVigencia)
                    .HasName("IX_PFechaVigencia");

                entity.HasIndex(e => e.Idempresa)
                    .HasName("IX_PEmpresa");

                entity.HasIndex(e => e.Idformulario)
                    .HasName("IX_PFormulario");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_PRegistro");

                entity.Property(e => e.Idpublicacion).HasColumnName("IDPublicacion");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.FechaVigencia).HasColumnType("datetime");

                entity.Property(e => e.Idempresa).HasColumnName("IDEmpresa");

                entity.Property(e => e.Idformulario).HasColumnName("IDFormulario");

                entity.Property(e => e.MensajeDespedida)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime");

                entity.Property(e => e.Usuario).HasColumnName("usuario");
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.HasKey(e => e.Idrespuesta);

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_RActivo");

                entity.HasIndex(e => e.Idcontrol)
                    .HasName("IX_RControl");

                entity.HasIndex(e => e.Idformulario)
                    .HasName("IX_RFormulario");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_RRegistro");

                entity.Property(e => e.Idrespuesta).HasColumnName("IDRespuesta");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Idcontrol).HasColumnName("IDControl");

                entity.Property(e => e.Idformulario).HasColumnName("IDFormulario");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime");

                entity.Property(e => e.Respuesta1)
                    .IsRequired()
                    .HasColumnName("Respuesta")
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.Urlqs)
                    .IsRequired()
                    .HasColumnName("URLQS")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario).HasColumnName("usuario");
            });

            modelBuilder.Entity<RespuestaValoracion>(entity =>
            {
                entity.HasKey(e => e.IdrespuestaValoracion);

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_RVActivo");

                entity.HasIndex(e => e.Idcontrol)
                    .HasName("IX_RVControl");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_RVRegistro");

                entity.Property(e => e.IdrespuestaValoracion).HasColumnName("IDRespuestaValoracion");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Idcontrol).HasColumnName("IDControl");

                entity.Property(e => e.PatterValidation)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime");

                entity.Property(e => e.RespuestaCorrecta)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario).HasColumnName("usuario");
            });

            modelBuilder.Entity<UserDbdefinition>(entity =>
            {
                entity.HasKey(e => e.IduserDb);

                entity.ToTable("UserDBDefinition");

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_UDBActivo");

                entity.HasIndex(e => e.EsPublicada)
                    .HasName("IX_UDBEsPublica");

                entity.HasIndex(e => e.Idempresa)
                    .HasName("IX_UDBEmpresa");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("IX_UDBIDUsuario");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_UDBRegistro");

                entity.Property(e => e.IduserDb).HasColumnName("IDUserDB");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Header)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Idempresa).HasColumnName("IDEmpresa");

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UserDbrows>(entity =>
            {
                entity.HasKey(e => e.IduserDbrow);

                entity.ToTable("UserDBRows");

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_UDBRActivo");

                entity.HasIndex(e => e.IduserDb)
                    .HasName("IX_UDBREmpresa");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_UDBRRegistro");

                entity.Property(e => e.IduserDbrow).HasColumnName("IDUserDBRow");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IduserDb).HasColumnName("IDUserDB");

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserDataRow)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UserEventLog>(entity =>
            {
                entity.HasKey(e => e.IdeventLog);

                entity.HasIndex(e => e.IdeventType)
                    .HasName("IX_UELEventType");

                entity.HasIndex(e => e.Idsesion)
                    .HasName("IX_UELIDSesion");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_UELRegistro");

                entity.HasIndex(e => e.UserTableName)
                    .HasName("IX_UELUserTableName");

                entity.Property(e => e.IdeventLog).HasColumnName("IDEventLog");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdeventType).HasColumnName("IDEventType");

                entity.Property(e => e.Idsesion).HasColumnName("IDSesion");

                entity.Property(e => e.LastValue).IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TextMessage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserTableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ValuePk).HasColumnName("ValuePK");
            });

            modelBuilder.Entity<UserSesion>(entity =>
            {
                entity.HasKey(e => e.Idsesion);

                entity.HasIndex(e => e.EsNormalLogout)
                    .HasName("IX_USLogout");

                entity.HasIndex(e => e.HoraFin)
                    .HasName("IX_USHoraFin");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("IX_USUsuario");

                entity.HasIndex(e => e.Registro)
                    .HasName("IX_USRegistro");

                entity.Property(e => e.Idsesion).HasColumnName("IDSesion");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.HoraFin)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

                entity.Property(e => e.Jwt)
                    .HasColumnName("JWT")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Jwtheader)
                    .HasColumnName("JWTHeader")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Jwtpayload)
                    .HasColumnName("JWTPayload")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Jwtsignature)
                    .HasColumnName("JWTSignature")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Idusuario);

                entity.HasIndex(e => e.Activo)
                    .HasName("IX_UActivo");

                entity.HasIndex(e => e.Idempresa)
                    .HasName("IX_UIDEmpresa");

                entity.HasIndex(e => e.Login)
                    .HasName("IX_ULogin");

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("apellidos")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Apodo)
                    .HasColumnName("apodo")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Idempresa)
                    .HasColumnName("IDEmpresa")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnName("nombres")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .IsUnicode(false);

                entity.Property(e => e.Registro)
                    .HasColumnName("registro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usuario1)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
