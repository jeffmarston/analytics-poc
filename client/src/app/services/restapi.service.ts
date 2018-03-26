import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class RestapiService {

  constructor(private http: HttpClient) { }

  public GetColumnsAsync(): any {
    return this.http.get('http://localhost:9000/serviceInfo/columns');
   // return this.http.get('http://localhost:6700/columns');
  }

}
