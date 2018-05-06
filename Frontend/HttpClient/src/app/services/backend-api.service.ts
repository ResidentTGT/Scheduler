import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { environment as env } from '../../environments/environment';
import { Detail } from '../models/detail';
import { RequestOptions, Headers } from '@angular/http';
import { Equipment } from '../models/equipment';
import { ProductionItem } from '../models/production-item';
import { Operation, OperationType } from '../models/operation';
import { Conveyor } from '../models/conveyor';
import { Workshop } from '../models/workshop';
import { Order } from '../models/order';
import { Route } from '../models/route';
import { OrderReport } from '../models/Reporting/OrderReport';


@Injectable()
export class BackendApiService {

    constructor(private http: Http) { }

    getDetails(pageNumber: number = 0, pageSize: number = 0, equipmentId: number = 0): Observable<Detail[]> {
        const query = `${env.backendUrl}api/details?pageNumber=${pageNumber}&pageSize=${pageSize}&equipmentId=${equipmentId}`;

        return this.http
            .get(query)
            .map(response => response.json() as Detail[]);
    }

    getWorkshops(pageNumber: number = 0, pageSize: number = 0): Observable<Workshop[]> {
        const query = `${env.backendUrl}api/workshops?pageNumber=${pageNumber}&pageSize=${pageSize}`;

        return this.http
            .get(query)
            .map(response => response.json() as Workshop[]);
    }

    getConveyors(pageNumber: number = 0, pageSize: number = 0): Observable<Conveyor[]> {
        const query = `${env.backendUrl}api/conveyors?pageNumber=${pageNumber}&pageSize=${pageSize}`;

        return this.http
            .get(query)
            .map(response => response.json() as Conveyor[]);
    }

    getDetailsWithRoutes(pageNumber: number = 0, pageSize: number = 0): Observable<Detail[]> {
        const query = `${env.backendUrl}api/details-with-routes?pageNumber=${pageNumber}&pageSize=${pageSize}`;

        return this.http
            .get(query)
            .map(response => response.json() as Detail[]);
    }

    getDetailsWithoutRoutes(): Observable<Detail[]> {
        return this.http
            .get(`${env.backendUrl}api/details-without-routes`)
            .map(response => response.json() as Detail[]);
    }

    createDetail(detail: Detail): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}api/create-detail`, JSON.stringify(detail), options)
            .map(resp => resp.json() as number);
    }

    createWorkshop(workshop: Workshop): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}api/create-workshop`, JSON.stringify(workshop), options)
            .map(resp => resp.json() as number);
    }

    createConveyor(conveyor: Conveyor): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}api/create-conveyor`, JSON.stringify(conveyor), options)
            .map(resp => resp.json() as number);
    }

    deleteDetail(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-detail?id=${id}`);
    }

    deleteWorkshop(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-workshop?id=${id}`);
    }

    deleteConveyor(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-conveyor?id=${id}`);
    }

    getEquipments(pageNumber: number = 0, pageSize: number = 0, operationType: number = OperationType.Undefined): Observable<Equipment[]> {
        const query = `${env.backendUrl}api/equipments?pageNumber=${pageNumber}&pageSize=${pageSize}&operationType=${operationType}`;

        return this.http
            .get(query)
            .map(response => response.json().map(e => Equipment.fromJSON(e)));
    }

    createEquipment(equipment: Equipment): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}api/create-equipment`, JSON.stringify(equipment), options)
            .map(resp => resp.json() as number);
    }

    deleteEquipment(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-equipment?id=${id}`);
    }

    getProductionItems(pageNumber: number = 0, pageSize: number = 0): Observable<ProductionItem[]> {
        const query = `${env.backendUrl}api/production-items?pageNumber=${pageNumber}&pageSize=${pageSize}`;

        return this.http
            .get(query)
            .map(response => response.json() as ProductionItem[]);
    }

    createProductionItem(productionItem: ProductionItem): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}api/create-production-item`, JSON.stringify(productionItem), options)
            .map(resp => resp.json() as number);
    }

    deleteProductionItem(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-production-item?id=${id}`);
    }

    getOperations(pageNumber: number = 0, pageSize: number = 0): Observable<Operation[]> {
        const query = `${env.backendUrl}api/operations?pageNumber=${pageNumber}&pageSize=${pageSize}`;

        return this.http
            .get(query)
            .map(response => response.json().map(e => Operation.fromJSON(e)));
    }

    getOperationsByDetailId(detailId: number, pageNumber: number = 0, pageSize: number = 0): Observable<Operation[]> {
        const query = `${env.backendUrl}api/detail-operations?detailId=${detailId}&pageNumber=${pageNumber}&pageSize=${pageSize}`;

        return this.http
            .get(query)
            .map(response => response.json().map(e => Operation.fromJSON(e)));
    }

    createOperation(operation: Operation): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}api/create-operation`, JSON.stringify(operation), options)
            .map(resp => resp.json() as number);
    }

    deleteOperation(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-operation?id=${id}`);
    }

    getOrders(pageNumber: number = 0, pageSize: number = 0): Observable<Order[]> {
        const query = `${env.backendUrl}api/orders?pageNumber=${pageNumber}&pageSize=${pageSize}`;
        return this.http
            .get(query)
            .map(response => response.json().map(e => Order.fromJSON(e)));
    }

    createOrder(order: Order): Observable<number> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${env.backendUrl}api/create-order`, JSON.stringify(order), options)
            .map(resp => resp.json() as number);
    }

    deleteOrder(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-order?id=${id}`);
    }

    calculateOrder(id: number | {}): Observable<string> {
        return this.http
            .get(`${env.backendUrl}api/calculate-order?id=${id}`)
            .map(resp => resp.json() as string);
    }

    getRoutes(pageNumber: number = 0, pageSize: number = 0): Observable<Route[]> {
        const query = `${env.backendUrl}api/routes?pageNumber=${pageNumber}&pageSize=${pageSize}`;
        return this.http
            .get(query)
            .map(response => response.json() as Route[]);
    }

    createRoute(route: Route): Observable<number> {
        return this.http.post(`${env.backendUrl}api/create-route`, route)
            .map(resp => resp.json() as number);
    }

    deleteRoute(id: number | {}) {
        return this.http
            .get(`${env.backendUrl}api/delete-route?id=${id}`);
    }

    getGraphOrderReport(orderId: number): Observable<OrderReport> {
        return this.http
            .get(`${env.backendUrl}api/order-report?id=${orderId}`)
            .map(resp => OrderReport.fromJSON(resp));
    }

}
