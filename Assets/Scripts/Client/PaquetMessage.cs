using System;
using System.Collections.Generic;
using System.Text;

namespace clientPackage
{
    public class PaquetMessage
    {
        public int IdJoueur { get; set; }
        public int IdAction { get; set; }
        public List<List<string>> Parametres { get; set; }


        public PaquetMessage(int IdJoueur, int IdAction, List<List<string>> Parametres)
        {
            this.IdJoueur = IdJoueur;
            this.IdAction = IdAction;
            this.Parametres = Parametres;
        }

        // ToString method to represent the PaquetMessage object as a string
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("IdJoueur: ").Append(IdJoueur).Append(" IdAction: ").Append(IdAction).Append(" Parametres: ");
            foreach (var list in Parametres)
            {
                sb.Append("(");
                foreach (var param in list)
                {
                    sb.Append(param).Append(", ");
                }
                sb.Remove(sb.Length - 2, 2); // Remove the trailing comma and space
                sb.Append(") ");
            }
            return sb.ToString();
        }
    }
}
