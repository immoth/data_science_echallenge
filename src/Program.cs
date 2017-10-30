using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("input the file name in the format \"filename.txt\"");
            string filename = Console.ReadLine();  //collects the file name from the console
            Console.WriteLine("This application assumes that the data entries all come from the same year \n as this is how they are stored on the SEC website");
            Console.WriteLine("Please inpute the four digit year the data was collected");
            int year = int.Parse(Console.ReadLine());  //collects the year from the console

            StreamReader sr = new StreamReader(filename);  //reads from the input file

            string line = "";  //a line of the input file 
            List<string> zipList = new List<string>();  //stores the zipcodes in the order they are read off
            List<int> amntList = new List<int>();  //stores the amounts in the order they are read off
            List<int> dateList = new List<int>();  //stores the dates in the order they are read off
            List<string> zipCond = new List<string>();  //a condensed list of zipcodeds used to check for repeats
            List<int> dateCond = new List<int>(); //a condensed list of dates used to check for repeats
            List<string> zipTP = new List<string>();  // The list that is printed to medianvals_by_zip.txt
            List<string> dateTP = new List<string>(); // An unsorted list of date information
            List<string> dateTPsort = new List<string>(); //Sorted list that is printed to medianvals_by_date.txt 
            List<int> amntZIPcond = new List<int>();  //list of amounts condensed according to zipCond
            List<int> totalZIPcond = new List<int>(); //list of totals condensed according to zipCond
            List<int> numZIPcond = new List<int>();  //the number of identical zip condes condensed according to zipCond
            List<int> amntDATEcond = new List<int>(); //list of amounts condensed according to dateCond
            List<int> totalDATEcond = new List<int>(); //list of totals condensed according to dateCond
            List<int> numDATEcond = new List<int>();  //the number of identical dates condensed according to dateCond

            int nt = 0;   //tracks the current line that is being read

            //begin reading the lines
            while ((line = sr.ReadLine()) != null) 
            {
                nt += 1;  //update the line number
                if (line.Count(x => x == '|') == 20)  //checks if the current line is formated correctly
                {
                    string[] arry = line.Split(new Char[] { '|' });  //collects relevent information into 'arry'
                    


                    string zip = "";  //stores the current zip code

                    //shortens zip code to five digits
                    if (arry[10].Length > 5)
                    {
                        zip = arry[10].Substring(arry[10].Length - 5);
                    }
                    else
                    {
                        zip = arry[10];
                    }
                    //end shorten


                    int tf;  //boolean object
                    int tff; //boolean object
                    if (arry[15] == "" && int.TryParse(arry[13], out tf) && int.TryParse(arry[10], out tff))  //check if OTHER_ID is empty and that the date and zipcode is formated correctely 
                    {
                        zipList.Add(arry[10]);   //saves zipcodes for later use
                        dateList.Add(int.Parse(arry[13]));  //saves the date for later use

                        //performs the calculations of mean, total, and num
                        //zipcode calculation
                        int numz = 1;  //the number of zipcodes identical to the current line
                        int totalz = int.Parse(arry[14]);  //the total amount donated from this zipcode
                        int meanz = int.Parse(arry[14]);  //the average amount donated from this zipcode
                        int addTozipC = 1;  //wheather or not the current zipcode is new 1=yes 0=no
                        
                        //scan through the condensed list of zip codes
                        for (int i = 0; i < zipCond.Count; i++)  
                        {
                            if (zipCond[i] == zip)  //checks if the current zipcode is a repeat
                            {
                                numZIPcond[i] += 1;  //updates the number of identical zipcodes
                                totalZIPcond[i] += int.Parse(arry[14]);  //updates the total from the current zipcode
                                addTozipC = 0; //the current zip code is not new
                                numz = numZIPcond[i]; //the number of zipcodes identical to the current line
                                totalz = totalZIPcond[i]; //the total amount donated from this zipcode
                            }
                        }
                        if (addTozipC == 1)  //if this zip code does not match any of the previous
                        {
                            zipCond.Add(zip);  //creates a new entery for the current zip code in zipCond
                            amntZIPcond.Add(int.Parse(arry[14]));  //creates a new entery for the amount donated from the current zipcode
                            numZIPcond.Add(1);  //creates a new entery for the number of zip codes identical to the current zipcode in zipCond
                            totalZIPcond.Add(int.Parse(arry[14])); //creates a new entery for the total amount donated from the current zipcode in zipCond
                        }
                        //end scan
                        meanz = totalz / numz; //the average amount donated from the current zipcode 
                        //end zipcode calculation

                        //date calculation
                        int numd = 1;  //the number of dates identical the current date
                        int totald = int.Parse(arry[14]);  //the total amount donated from the current date
                        int meand = int.Parse(arry[14]); //the average amount donated from the current date
                        int addTOdateC = 1;  //wheather or not the current date is new yes=1 no=0

                        //scan through the condensed list of dates
                        for (int i = 0; i < dateCond.Count; i++)
                        {
                            if (dateCond[i] == int.Parse(arry[13]))  //checks if the current date has already accurred 
                            {
                                numDATEcond[i] += 1;  //updates the number of dates identical the the current date
                                totalDATEcond[i] += int.Parse(arry[14]); //updates the total amound donated from the current date
                                addTOdateC = 0; //the current date is not new
                                numd = numDATEcond[i]; //the number of dates identical the the current date
                                totald = totalDATEcond[i]; //the total amount donated from the current date
                                dateTP[i] = string.Join("|", arry[0], arry[13], meand, numd, totald); //updates the line to be printed to the output
                            }
                        }
                        if (addTOdateC == 1)  //if the current date is not identical to any others so far
                        {
                            dateCond.Add(int.Parse(arry[13]));  //creates an entery for the current date
                            amntDATEcond.Add(int.Parse(arry[14])); //creates an entery for the amount donated from the current date
                            numDATEcond.Add(1); //creates an entery for the number of date identical to the current one
                            totalDATEcond.Add(int.Parse(arry[14]));  //creates an entery for the total amount donated from the current date
                            dateTP.Add(string.Join("|", arry[0], arry[13], meand, numd, totald)); //updates the line to be printed to the output
                        }
                        //end scan
                        meand = totald / numd;  //the average amount donated from the current output
                        //end date calculation
                        //end calculation


                        zipTP.Add(string.Join("|", arry[0], zip, meanz, numz, totalz));  //updates the zipcode list to be printed to the output




                        //writes the line number, the current number of unique zipcodes, and the current number of unique dates
                        if (nt % 1000 == 0)
                        {
                            Console.WriteLine("{0}:{1}:{2}", nt, zipCond.Count, dateCond.Count);
                        }
                        //end write
                    }
                }
            }
            //end reading lines

            //sort dateTP
            int l = 0;
            for (int m = 1; m <= 12; m++) 
            {
                 for (int d = 1; d <= 31; d++)
                 {
                    for (int a = l; a < dateCond.Count; a++) 
                    {
                        if (dateCond[a] < m * 1000000 + d * 10000+year)
                        {
                            int dateCondtemp = dateCond[a];
                            dateCond.RemoveAt(a);
                            if (l <= dateCond.Count) { dateCond.Insert(l, dateCondtemp); } else { dateCond.Add(dateCondtemp); }
                            string dateTPtemp = dateTP[a];
                            dateTP.RemoveAt(a);
                            if (l <= dateTP.Count) { dateTP.Insert(l, dateTPtemp); } else { dateTP.Add(dateTPtemp); }
                            l += 1;
                        }
                    }
                 }
            }
            //end sort
           
            sr.Close();

            //Writing medianvals_by_zip
            string zipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\WorldOrProgrammers";

            if (!Directory.Exists(zipDirectory)) { Directory.CreateDirectory(zipDirectory); }

            using (StreamWriter sw = new StreamWriter(zipDirectory + "\\medianvals_by_zip.txt"))
            {
                sw.WriteLine("{0}", String.Join(Environment.NewLine, zipTP.ToArray()));
            }
            //end writing 

            //Writing medianvals_by_date
            string dateDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\WorldOrProgrammers";

            if (!Directory.Exists(dateDirectory)) { Directory.CreateDirectory(dateDirectory); }

            using (StreamWriter sw = new StreamWriter(dateDirectory + "\\medianvals_by_date.txt"))
            {
                sw.WriteLine("{0}", String.Join(Environment.NewLine, dateTP.ToArray()));
            }
            //end writing 

            Console.ReadLine();

        }
    }
}
