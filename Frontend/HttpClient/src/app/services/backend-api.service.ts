import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { environment as env } from '../../environments/environment';
import { Detail } from '../models/detail';
import { RequestOptions, Headers } from '@angular/http';


@Injectable()
export class BackendApiService {

    constructor(private http: Http) { }

    getDetails(): Observable<Detail[]> {
        return this.http
            .get(`${env.backendUrl}details`)
            .map(response => response.json() as Detail[]);
    }

    createDetail(detail: Detail): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}create-detail`, JSON.stringify(detail), options)
            .map(resp => resp.json() as number);

    }
}
