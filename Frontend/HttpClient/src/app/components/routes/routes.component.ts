import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Route } from '../../models/route';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Rx';
import { CreateRouteComponent } from '../create-route/create-route.component';

@Component({
    selector: 'sch-routes',
    templateUrl: './routes.component.html',
    styleUrls: ['./routes.component.scss'],
})
export class RoutesComponent implements OnInit {

    public routes: Route[] = [];

    constructor(private _api: BackendApiService, private dialog: MatDialog) { }

    ngOnInit() {
        this.getRoutes();
    }

    private getRoutes() {
        this._api.getRoutes()
            .do(routes => this.routes = routes)
            .catch(resp => {
                alert(`Не удалось загрузить список маршрутов по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public deleteRoute(route: Route) {
        this._api.deleteRoute(route.id)
            .catch(resp => {
                alert(`Не удалось удалить маршрут по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(_ => this.routes.splice(this.routes.indexOf(route), 1));
    }

    public openDialog() {
        this.dialog.open(CreateRouteComponent, { data: { routes: this.routes } });
    }

}
