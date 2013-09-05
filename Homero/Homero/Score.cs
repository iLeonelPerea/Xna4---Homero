using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Homero
{
    public class Score
    {
        private int puntos;
        private string nombre;
        public Score(string n, int s)
        {
            puntos = s;
            nombre = n;
        }

        public int Puntos
        {
            set { puntos = value; }
            get { return puntos; }
        }
        public string Nombre {
            set { nombre = value; }
            get { return nombre; }
        }
    }
}
