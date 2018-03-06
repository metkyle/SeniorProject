using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookOrder.Core.Models;
using BookOrder.Repositories.Courses;
using System.Collections;
using System.IO;
using System.Net.Http;
using FileHelpers;


/// <summary>
/// Parses a CSV file and stores the information into a CSVCourse[]
/// </summary>
/// 
namespace BookOrder.Repositories.Courses
{

public class CSVParser
{   
   private FileHelpers.FileHelperEngine<CSVCourse> engine;
   private CSVCourse[] records;
    public String filePath;

    public CSVParser(String theFilePath)
    {
        engine = new FileHelpers.FileHelperEngine<CSVCourse>();
        filePath = theFilePath;
        records = engine.ReadFile(theFilePath);
        
    }

    public CSVParser()
    {
        engine = new FileHelpers.FileHelperEngine<CSVCourse>();
        
    }


    public CSVCourse[] getRecords()
    {
        return this.records;
    }

   /* public void exportCSV(CSVCourse[] theRecords){
        var engine = new FileHelpers.FileHelperEngine<CSVCourse>();
        //theParser = new CSVParser(filePath);
        //theRecords = theParser.getRecords();

        engine.WriteFile("CSVOUTPUT.txt", theRecords);
     }*/
    
    
    }
}