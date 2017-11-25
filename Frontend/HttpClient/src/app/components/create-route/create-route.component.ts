import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Operation } from '../../models/operation';
import { Detail } from '../../models/detail';
import { Observable } from 'rxjs/Rx';
import { Route } from '../../models/route';

@Component({
    selector: 'sch-create-route',
    templateUrl: './create-route.component.html',
    styleUrls: ['./create-route.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CreateRouteComponent implements OnInit {

    public name: string;
    public description: string;
    public addedOperations: Operation[] = [];
    public detail: Detail;
    public operation: Operation;

    public viewOperations: Operation[] = [];
    public operations: Operation[] = [];
    public details: Detail[] = [];
    public routes: Route[] = [];

    constructor(private _api: BackendApiService
        , public matDialogRef: MatDialogRef<CreateRouteComponent>
        , @Inject(MAT_DIALOG_DATA) data: any) {
        this.routes = data.routes;
    }

    ngOnInit() {
        this.getDetailsWithoutRoutes();
        this.getOperations();
    }

    public createRoute() {
        const route: Route = {
            description: this.description,
            name: this.name,
            operations: this.addedOperations,
            detail: this.detail,
        };

        this._api.createRoute(route)
            .catch(resp => {
                alert(`Не удалось добавить маршрут по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                route.id = id;
                this.routes.push(route);
                this.closeDialog();
            });
    }

    private getDetailsWithoutRoutes() {
        this._api.getDetailsWithoutRoutes()
            .do(details => this.details = details)
            .catch(resp => {
                alert(`Не удалось загрузить список деталей по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    private getOperations() {
        this._api.getOperations()
            .do(operations => this.operations = operations)
            .catch(resp => {
                alert(`Не удалось загрузить список операций по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public addOperation() {
        this.addedOperations.push(this.operation);
        this.viewOperations.splice(this.viewOperations.indexOf(this.operation), 1);

        this.operation = null;
    }

    public filterOperations(detail: Detail) {
        if (this.operations) {
            this.viewOperations = this.operations.filter(o => o.detail.id === detail.id);
        }
    }

    closeDialog() {
        this.matDialogRef.close();
    }

}