import { Component, OnInit, Input } from '@angular/core';
import {Http, RequestOptions, Headers, Response} from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-file-upload-component',
  templateUrl: './file-upload-component.component.html',
  styleUrls: ['./file-upload-component.component.css']
})
export class FileUploadComponentComponent implements OnInit {

  
  private filePath: String;
  private isUploadBtn: boolean = true;
  private apiEndpoint: string = 'api/FileUploadProcessor';
  private courseInfo: String[];

  constructor(private http: Http, private snackbar: MatSnackBar) {
    
   }
   //file upload event
   fileChange(event){
    // debugger;
     let fileList: FileList = event.target.files;
     if(fileList.length > 0){
       let file: File = fileList[0];
       let formData: FormData = new FormData();
       formData.append('uploadFile', file, file.name);
       
       let headers = new Headers();
       let options = new RequestOptions({headers: headers});
       let apiUrl1 = this.apiEndpoint;
       console.log(apiUrl1);
       console.log(this.http.post.name);
       this.http.post(apiUrl1, formData, options)
         .map(res => res.json())
         .catch(error => Observable.throw(error))
         .subscribe(data => this.snackbar.open("File Uploaded Successfully", '', { duration: 2000 }), error => this.snackbar.open("Error Uploading File (.csv files only/6 column csv)", '', { duration: 2000 }));
         
     }
    
   }

   exportCSV(): void {

   }
  ngOnInit() {
   }

  getCSVObject(): void {
    let options = new RequestOptions({
      headers: new Headers({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      })
    });
    this.http.get(this.apiEndpoint, options).subscribe((res: Response) => this.courseInfo = res.json());
    
  }

  
}
