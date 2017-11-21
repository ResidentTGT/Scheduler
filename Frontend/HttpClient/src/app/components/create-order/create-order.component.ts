import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Order, OrderState } from '../../models/order';
import { Observable } from 'rxjs/Rx';
import { ProductionItem } from '../../models/production-item';
import { OrderQuantum } from '../../models/order-quantum';

@Component({
    selector: 'sch-create-order',
    templateUrl: './create-order.component.html',
    styleUrls: ['./create-order.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CreateOrderComponent implements OnInit {
    public name: string;
    public description: string;
    public state: string = OrderState[OrderState.Undefined];
    public plannedBeginDate: Date;
    public plannedEndDate: Date;

    public productionItem: ProductionItem;
    public itemsCountInOnePart: number;
    public productionItemsCount: number;

    public orders: Order[] = [];
    public orderQuantums: OrderQuantum[] = [];

    public typeOptions: string[] = [];
    public productionItems: ProductionItem[] = [];

    constructor(private _api: BackendApiService
        , public matDialogRef: MatDialogRef<CreateOrderComponent>
        , @Inject(MAT_DIALOG_DATA) data: any) {
        this.orders = data.orders;
    }

    ngOnInit() {
        this.getProductionItems();
        this.typeOptions = Object.keys(OrderState);
        this.typeOptions = this.typeOptions.slice(this.typeOptions.length / 2);
    }

    public createOrder() {
        const order: Order = {
            description: this.description,
            name: this.name,
            plannedBeginDate: this.plannedBeginDate,
            plannedEndDate: this.plannedEndDate,
            state: this.state || OrderState[this.state],
            orderQuantums: this.orderQuantums
        };

        this._api.createOrder(order)
            .catch(resp => {
                alert(`Не удалось добавить заказ по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                order.id = id;
                this.orders.push(order);
                this.closeDialog();
            });
    }

    private getProductionItems() {
        this._api.getProductionItems()
            .do(productionItems => this.productionItems = productionItems)
            .catch(resp => {
                alert(`Не удалось загрузить список продукции по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public addProductionItemPart() {
        const orderQuantum: OrderQuantum = {
            count: this.productionItemsCount,
            itemsCountInOnePart: this.itemsCountInOnePart,
            productionItem: this.productionItem
        };
        this.orderQuantums.push(orderQuantum);

        this.productionItems.splice(this.productionItems.indexOf(this.productionItem), 1);
        this.productionItem = null;
    }

    closeDialog() {
        this.matDialogRef.close();
    }
}
