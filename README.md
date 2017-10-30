# data_science_echallenge

Running the program:  
                      The console will ask for an input file name which should be a text file and needs to end .txt (example itcont.txt).  
                      Next it will ask for the year the data was collected.  This is to remind the user that only one year can be processed 
                      at a time.  The program should run fine even if the wrong year is used.
                     
Efficiency:          
                      The program reads a line and then checks if a previous read line already has the same date or zipcode.  The maximum 
                      number of dates that need to be scanned is 365 but the number of zipcodes is in the 10000s.  Once the number of new 
                      zipcodes saturates at around line one million, the run time becomes linear with input size.  
                      
