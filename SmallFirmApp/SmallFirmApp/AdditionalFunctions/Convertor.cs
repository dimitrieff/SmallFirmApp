using DocumentFormat.OpenXml.Wordprocessing;

namespace SmallFirmApp.AdditionalFunctions
{
    public class Convertor
    {
        public static string ConvertToLetter (double inValue)
        {
            string outNumber = "";
            string outValue;

            int inIntPart = (int)Math.Truncate(inValue);//cqla stojnost

            double frac = Math.Round((inValue % 1) * 100);//ostatyk

            //разбивка на левовете

            string lewa = inIntPart.ToString();

            int numberOfDigits = lewa.Length;

            if (inIntPart == 0)//prowerka za nulwa stojnost
            {
                outNumber = "нула";
            }
            else
            {
                for (int i = 0; i < numberOfDigits; i++)
                {
                    outNumber = outNumber + OutResult(i, lewa);
                }
            }
            //okon4atelen izhod

            if (frac != 0)
            {
                outValue = outNumber.Trim() + " лв. и " + frac.ToString() + " ст.";
            }
            else
            {
                outValue = outNumber.Trim() + " лв.";
            }
            return (outValue);
        }

        private static string OutResult(int position, string value)
        {
            string[] TextTosands = { "хиляда", "две хиляди", "три хиляди", "четири хиляди", "пет хиляди",
            "шест хиляди", "седем хиляди", "осем хиляди", "девет хиляди"};
            string[] TextHundreds = {"сто","двеста", "триста", "четиристотин", "петстотин",
                "шестотин", "седемстотин", "осемстотин", "деветстотин"};
            string[] TextDecades = {"десет", "двадесет", "тридесет", "четиридесет", "петдесет", "шестдесет",
                    "седемдесет", "осемдесет", "деветдесет" };
            string[] TextDigits = { "нула", "един", "два", "три", "четири", "пет", "шест", "седем", "осем", "девет" };
            string[] Text1020 = {"десет", "единадесет", "дванадесет", "тринадесет", "четиренадесет", "петнадесет",
            "шестнадесет", "седемнадесет", "осемнадесет", "деветнадесет"};

            string result = ""; //rezultat

            char temp = value[position];
            string temp1 = temp.ToString();
            int vpos = Convert.ToInt32(temp1);//teku]a stojnost na teku]a poziciq

            int count = value.Length;//poziciq otzad napred

            int currentSign = count - position;//teku]a poziciq otpred nazad

            int tempo = 0;//stojnost na predposleden znak

            if (count > 1)//opredelqne na preposledniq znak
            {
                string temp2 = value.Substring(value.Length - 2);
                char tempOne = temp2[0];
                string TempTwo = tempOne.ToString();
                tempo = Convert.ToInt32(TempTwo);
            }
            if ((currentSign == 4) && (vpos > 0))//prewod na hilqdi
            {
                int last1 = Convert.ToInt32(value.Substring(value.Length - 1));
                int last2 = Convert.ToInt32(value.Substring(value.Length - 2));
                int last3 = Convert.ToInt32(value.Substring(value.Length - 3));

                if ((last3 == 0) || (last1 != 0 && last2 > 9) || (last3 > 100) && (last2 != 0))
                {
                    result = TextTosands[vpos - 1] + " "; // + " и";
                }
                else
                {
                    result = TextTosands[vpos - 1] + " и ";
                }
            }
            if ((currentSign == 3) && (vpos > 0))//prewod na stotni
            {
                int last1 = Convert.ToInt32(value.Substring(value.Length - 1));
                int last2 = Convert.ToInt32(value.Substring(value.Length - 2));

                if ((last2 == 0) || ((last1 != 0) && (last2 > 9)))
                {
                    result = TextHundreds[vpos - 1] + " ";
                }
                else
                {
                    result = TextHundreds[vpos - 1] + " и ";
                }
            }
            if ((currentSign == 2) && (vpos == 1))//prewod deseti mevdu 10 i 20
            {
                int last1 = Convert.ToInt32(value.Substring(value.Length - 1));
                if (count > 2 && last1 != 0)
                {
                    result = "и " + Text1020[last1];
                }
                else
                {
                    result = Text1020[last1];
                }
            }
            if ((currentSign == 2) && (vpos > 1))//prewod deseti 20 - 90
            {
                int temp2 = Convert.ToInt32(value.Substring(value.Length - 1));


                if (temp2 != 0)
                {
                    result = TextDecades[vpos - 1] + " и ";
                }
                else
                {
                    result = TextDecades[vpos - 1];
                }
            }
            if ((currentSign == 1) && (vpos > 0))//prewod edinici
            {

                if (tempo != 1)
                {
                    result = TextDigits[vpos];
                }
            }
            return result;//wry6tane rezultat
        }
    }
}
