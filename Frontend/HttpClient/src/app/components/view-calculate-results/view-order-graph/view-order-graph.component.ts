import { Component, OnInit } from '@angular/core';
import { Order } from '../../../models/order';
import { BackendApiService } from '../../../services/backend-api.service';
import { Observable } from 'rxjs/Rx';
import { OrderReport } from '../../../models/Reporting/OrderReport';
import { ActivatedRoute } from '@angular/router';
import { OrderBlock } from '../../../models/Reporting/OrderBlock';
import { GroupBlock } from '../../../models/Reporting/GroupBlock';
import { Workshop } from '../../../models/workshop';

@Component({
    selector: 'sch-view-order-graph',
    templateUrl: './view-order-graph.component.html',
    styleUrls: ['./view-order-graph.component.scss']
})
export class ViewOrderGraphComponent implements OnInit {

    public loading: boolean;

    public selectedBlock: OrderBlock;

    public selectedGroup: { group: GroupBlock, workshop: Workshop };

    public report: OrderReport;

    constructor(private _api: BackendApiService, private _activatedRoute: ActivatedRoute, ) { }

    ngOnInit() {
        this._activatedRoute.params
            .map(params => +params['id'])
            .switchMap(orderId => {
                return isNaN(orderId)
                    ? Observable.of(new OrderReport())
                    : this.getGraphOrderReport(orderId);
            }).subscribe(r => this.report = r);
    }

    getGraphOrderReport(orderId: number): Observable<OrderReport> {
        this.loading = true;
        return this._api.getGraphOrderReport(orderId)
            .do(r => {
                this.report = r;
                this.loading = false;
            })
            .catch(resp => {
                this.loading = false;
                alert(`Не удалось загрузить отчет по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.of(new OrderReport());
            });
    }

}
