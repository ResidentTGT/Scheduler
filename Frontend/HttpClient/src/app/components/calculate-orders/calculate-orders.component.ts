import { Component, OnInit, ViewEncapsulation, ElementRef } from '@angular/core';
import { Order } from '../../models/order';
import { BackendApiService } from '../../services/backend-api.service';
import { Observable } from 'rxjs/Rx';

@Component({
    selector: 'sch-calculate-orders',
    templateUrl: './calculate-orders.component.html',
    styleUrls: ['./calculate-orders.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CalculateOrdersComponent implements OnInit {

    public orders: Order[] = [];
    public loading: boolean;
    public selectedOrderId: number | {};

    public selectedBlock: { orderQuantumIndex: number, offset: number };

    public selectedGroup: { orderQuantumIndex: number, groupIndex: number, offset: number };

    public calculatedOrder: Order = null;

    constructor(private _api: BackendApiService) { }

    ngOnInit() {
        this.getOrders();
    }

    private getOrders() {
        this.loading = true;
        this._api.getOrders()
            .do(orders => this.orders = orders)
            .catch(resp => {
                this.loading = false;
                alert(`Не удалось загрузить список заказов по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .subscribe(_ => this.loading = false);
    }

    public selectOrder(id: number) {
        this.selectedOrderId = id;
    }

    private calculateOrder() {
        this.calculatedOrder = null;
        this.selectedBlock = null;
        this.selectedGroup = null;
        this._api.calculateOrder(this.selectedOrderId).subscribe(
            order => {
                this.calculatedOrder = order;
            });
    }

}
