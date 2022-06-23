import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {

  constructor(private http:HttpClient) { }

  departments:any=[];
  employees:any=[];

  modalTitle = "";
  EmployeeId = 0;
  EmployeeName = "";
  Department = "";
  DateOfJoining = "";
  PhotoFileName = "1.png";
  PhotoPath = environment.PHOTO_URL;

  ngOnInit(): void {
    this.refreshList();
  }

  getDepartmentList(){
    this.http.get<any>(environment.API_URL+'department')
    .subscribe(data=>{
      this.departments=data;
    });
  }

  refreshList(){
    this.http.get<any>(environment.API_URL+'Employee')
    .subscribe(data=>{
      this.employees=data;
    });
    this.getDepartmentList();
  }

  addClick(){
    this.modalTitle = "Add Employee";
    this.EmployeeId = 0;
    this.EmployeeName = "";
    this.Department = "";
    this.DateOfJoining = "";
    this.PhotoFileName = "1.png";
  }

  editClick(emp:any){
    this.modalTitle = "Edit Employee";
    this.EmployeeId = emp.EmployeeId;
    this.EmployeeName = emp.EmployeeName;
    this.Department = emp.Department;
    this.DateOfJoining = emp.DateOfJoining;
    this.PhotoFileName = emp.PhotoFileName;
  }

  createClick(){
    var val={
      EmployeeName:this.EmployeeName,
      EmployeeId:this.EmployeeId,
      Department:this.Department,
      DateOfJoining:this.DateOfJoining,
      PhotoFileName:this.PhotoFileName
    };
    this.http.post(environment.API_URL+'Employee',val)
    .subscribe(res=>{
      alert(res.toString());
      this.refreshList();
    });
  }

  updateClick(){
    var val={
      EmployeeName:this.EmployeeName,
      EmployeeId:this.EmployeeId,
      Department:this.Department,
      DateOfJoining:this.DateOfJoining,
      PhotoFileName:this.PhotoFileName
    };
    this.http.put(environment.API_URL+'Employee',val)
    .subscribe(res=>{
      alert(res.toString());
      this.refreshList();
    });
  }

  deleteClick(id:any){
    if(confirm('Are you sure ?')){
      this.http.delete(environment.API_URL+'Employee/'+id)
      .subscribe(res=>{
        alert(res.toString());
        this.refreshList();
      });
    }
  }

  imageUpload(event:any){
    var file=event.target.files[0];
    const formData:FormData=new FormData();
    formData.append('file',file,file.name);
    this.http.post(environment.API_URL+'employee/SaveFile',formData)
    .subscribe((data:any)=>{
      this.PhotoFileName = data.toString();
    });
  }

  dateToDDMMYYYY(dt:any) {
    var pipe = new DatePipe('en-US');
    var d = new Date(dt);
    var formattedDate = pipe.transform(dt, 'dd-MM-yyyy');
    return formattedDate;
    //return moment(d).format("D-MM-yyyy");
}

}
