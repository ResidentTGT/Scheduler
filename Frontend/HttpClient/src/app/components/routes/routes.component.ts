import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Route } from '../../models/route';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Rx';
import { CreateRouteComponent } from '../create-route/create-route.component';
import { environment as env } from '../../../environments/environment';
import { DataSource } from '@angular/cdk/collections';
import { Router } from '@angular/router';

@Component({
    selector: 'sch-routes',
    templateUrl: './routes.component.html',
    styleUrls: ['./routes.component.scss'],
})
export class RoutesComponent implements OnInit {

    public routes: Route[] = [];
    public dataSource: RoutesDataSource | null;
    public displayedColumns = ['name', 'description', 'detail', 'operations-count', 'openButton', 'deleteButton'];

    public pageSizeOptions: number[] = env.pageSizeOptions;
    public pageNumber = 0;
    public pageSize: number = env.pageSizeOptions[0];
    public pageLength: number;

    public loading: boolean;

    constructor(private _api: BackendApiService, private _router: Router) { }

    ngOnInit() {
        this.getRoutes(this.pageNumber, this.pageSize).subscribe();
    }

    private getRoutes(pageNumber: number, pageSize: number): Observable<Route[] | {}> {
        this.loading = true;
        return this._api.getRoutes(pageNumber, pageSize)
            .do(routes => {
                this.routes = routes;
                this.dataSource = new RoutesDataSource(this.routes);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список маршрутов по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            });
    }

    public deleteRoute(route: Route) {
        this._api.deleteRoute(route.id)
            .catch(resp => {
                alert(`Не удалось удалить маршрут по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getRoutes(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getRoutes(this.pageNumber, this.pageSize).subscribe();
    }

    updatePaginatorFields() {
        this.pageLength = (this.routes.length < this.pageSize)
            ? this.routes.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
    }

    public createRoute(): void {
        this._router.navigateByUrl('routes/create');
    }
}

export class RoutesDataSource extends DataSource<Route> {
    constructor(private _routes: Route[]) {
        super();
    }
    connect(): Observable<Route[]> {
        return Observable.of(this._routes);
    }
    disconnect() {
    }
}
