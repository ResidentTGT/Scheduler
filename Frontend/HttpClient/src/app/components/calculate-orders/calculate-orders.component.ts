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
    public selectedOrder: number;

    public selectedBlock: { orderQuantumIndex: number, blockIndex: number };

    public selectedGroup: { orderQuantumIndex: number, groupIndex: number };

    public calculatedOrder: Order = null;

    constructor(private _api: BackendApiService, private _elementRef: ElementRef) { }

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
        this.calculatedOrder = null;
        this.selectedBlock = null;
        this.selectedGroup = null;
        this._api.calculateOrder(this.selectedOrder).subscribe(
            order => {
                this.calculatedOrder = order;
            });
    }

    public selectOrder(id: number) {

        // const elems = this._elementRef.nativeElement.querySelectorAll('.selector');
        // elems.forEach(e => e.removeAttribute('checked'));
        this.selectedOrder = id;
    }
}
