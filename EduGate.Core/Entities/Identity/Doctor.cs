using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities.Identity
{
    public class Doctor : AppUser
    {
        public override string PictureUrl
        {
            get { return null; }
            set { }
        }
    }
}
