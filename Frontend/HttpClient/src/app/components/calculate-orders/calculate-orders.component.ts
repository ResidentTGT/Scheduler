import { Component, OnInit, ViewEncapsulation } from '@angular/core';
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

    public calculatedOrder: Order;

    constructor(private _api: BackendApiService) { }

    ngOnInit() {
        this.getOrders();
    }

    private getOrders() {
        this._api.getOrders()
            .do(orders => this.orders = orders)
            .catch(resp => {
                alert(`Не удалось загрузить список заказов по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    private calculateOrder() {
        this._api.calculateOrder(this.orders[0].id).subscribe(
            order => {
                this.calculatedOrder = order;
                alert('Заказ расчитан.');
            });
    }

}
