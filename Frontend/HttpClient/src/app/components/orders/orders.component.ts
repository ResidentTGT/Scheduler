import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialog } from '@angular/material';
import { Order } from '../../models/order';
import { Observable } from 'rxjs/Rx';
import { CreateOrderComponent } from '../create-order/create-order.component';

@Component({
    selector: 'sch-orders',
    templateUrl: './orders.component.html',
    styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {

    public orders: Order[] = [];

    constructor(private _api: BackendApiService, private dialog: MatDialog) { }

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

    public deleteOrder(order: Order) {
        this._api.deleteOrder(order.id)
            .catch(resp => {
                alert(`Не удалось удалить заказ по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(_ => this.orders.splice(this.orders.indexOf(order), 1));
    }

    public openDialog() {
        this.dialog.open(CreateOrderComponent, { data: { orders: this.orders } });
    }

}
