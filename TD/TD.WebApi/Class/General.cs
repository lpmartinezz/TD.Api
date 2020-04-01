using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TD.WebApi.Models;

namespace TD.WebApi.Class
{
    public class General : ControllerBase
    {
        private readonly FEV2Context _context;

        public General(FEV2Context context)
        {
            _context = context;
        }

        public bool ControlExistis(long id)
        {
            return _context.Control.Any(e => e.Idcontrol == id);
        }

        public bool FormularioExists(long id)
        {
            return _context.Formulario.Any(e => e.Idformulario == id);
        }

    }
}