import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialog } from '@angular/material';
import { Order } from '../../models/order';
import { Observable } from 'rxjs/Rx';
import { CreateOrderComponent } from '../create-order/create-order.component';
import { environment as env } from '../../../environments/environment';
import { DataSource } from '@angular/cdk/collections';
import { Router } from '@angular/router';

@Component({
    selector: 'sch-orders',
    templateUrl: './orders.component.html',
    styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {

    public orders: Order[] = [];
    public dataSource: OrdersDataSource | null;
    public displayedColumns = ['name', 'description', 'state', 'begin-date', 'end-date', 'products', 'openButton', 'deleteButton'];

    public pageSizeOptions: number[] = env.pageSizeOptions;
    public pageNumber = 0;
    public pageSize: number = env.pageSizeOptions[0];
    public pageLength: number;

    public loading: boolean;

    constructor(private _api: BackendApiService, private _router: Router) { }

    ngOnInit() {
        this.getOrders(this.pageNumber, this.pageSize).subscribe();
    }

    private getOrders(pageNumber: number, pageSize: number): Observable<Order[] | {}> {
        this.loading = true;
        return this._api.getOrders(pageNumber, pageSize)
            .do(orders => {
                this.orders = orders;
                this.dataSource = new OrdersDataSource(this.orders);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список заказов по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            });
    }

    public deleteOrder(order: Order) {
        this._api.deleteOrder(order.id)
            .catch(resp => {
                alert(`Не удалось удалить заказ по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getOrders(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getOrders(this.pageNumber, this.pageSize).subscribe();

    }

    updatePaginatorFields() {
        this.pageLength = (this.orders.length < this.pageSize)
            ? this.orders.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
    }

    public createOrder(): void {
        this._router.navigateByUrl('orders/create');
    }

}

export class OrdersDataSource extends DataSource<Order> {
    constructor(private _orders: Order[]) {
        super();
    }

    connect(): Observable<Order[]> {
        return Observable.of(this._orders);
    }

    disconnect() {
    }
}
