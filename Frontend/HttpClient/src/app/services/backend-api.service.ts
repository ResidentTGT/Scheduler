import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { environment as env } from '../../environments/environment';
import { Detail } from '../models/detail';
import { RequestOptions, Headers } from '@angular/http';
import { Equipment } from '../models/equipment';
import { ProductionItem } from '../models/production-item';
import { Operation } from '../models/operation';
import { Conveyor } from '../models/conveyor';
import { Workshop } from '../models/workshop';
import { Order } from '../models/order';


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

    deleteDetail(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}delete-detail?id=${id}`);
    }

    getEquipments(): Observable<Equipment[]> {
        return this.http
            .get(`${env.backendUrl}equipments`)
            .map(response => response.json().map(e => Equipment.fromJSON(e)));
    }

    createEquipment(equipment: Equipment): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}create-equipment`, JSON.stringify(equipment), options)
            .map(resp => resp.json() as number);
    }

    deleteEquipment(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}delete-equipment?id=${id}`);
    }

    getProductionItems(): Observable<ProductionItem[]> {
        return this.http
            .get(`${env.backendUrl}production-items`)
            .map(response => response.json() as ProductionItem[]);
    }

    createProductionItem(productionItem: ProductionItem): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}create-production-item`, JSON.stringify(productionItem), options)
            .map(resp => resp.json() as number);
    }

    deleteProductionItem(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}delete-production-item?id=${id}`);
    }

    getOperations(): Observable<Operation[]> {
        return this.http
            .get(`${env.backendUrl}operations`)
            .map(response => response.json().map(e => Operation.fromJSON(e)));
    }

    createOperation(operation: Operation): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}create-operation`, JSON.stringify(operation), options)
            .map(resp => resp.json() as number);
    }

    deleteOperation(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}delete-operation?id=${id}`);
    }

    getConveyors(): Observable<Conveyor[]> {
        return this.http
            .get(`${env.backendUrl}conveyors`)
            .map(response => response.json() as Conveyor[]);
    }

    getWorkshops(): Observable<Workshop[]> {
        return this.http
            .get(`${env.backendUrl}workshops`)
            .map(response => response.json() as Workshop[]);
    }

    getOrders(): Observable<Order[]> {
        return this.http
            .get(`${env.backendUrl}orders`)
            .map(response => response.json().map(e => Order.fromJSON(e)));
    }

    createOrder(order: Order): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}create-order`, JSON.stringify(order), options)
            .map(resp => resp.json() as number);
    }

    deleteOrder(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}delete-order?id=${id}`);
    }

}
