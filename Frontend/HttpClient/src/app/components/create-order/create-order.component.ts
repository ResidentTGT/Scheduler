import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Order, OrderState } from '../../models/order';
import { Observable } from 'rxjs/Rx';
import { ProductionItem } from '../../models/production-item';
import { OrderQuantum } from '../../models/order-quantum';
import { ProductionItemsDataSource } from '../production-items/production-items.component';
import { environment as env } from '../../../environments/environment';
import { DataSource } from '@angular/cdk/collections';
import { HelperService } from '../../services/helper.service';

@Component({
    selector: 'sch-create-order',
    templateUrl: './create-order.component.html',
    styleUrls: ['./create-order.component.scss'],
})
export class CreateOrderComponent implements OnInit {
    public name: string;
    public description: string;
    public plannedBeginDate: Date;
    public plannedEndDate: Date;
    public itemsCountInOnePart: number;
    public productionItemsCount: number;
    public orderQuantums: OrderQuantum[] = [];
    public orderQuantumsDataSource: OrderQuantumsDataSource | null;
    public orderQuantumsDisplayedColumns = ['title', 'count', 'count-in-batch', 'delete-button'];

    public productionItemsDataSource: ProductionItemsDataSource | null;
    public productionItemsDisplayedColumns = ['title', 'description'];
    public productionItemsPageNumber = 0;
    public productionItemsPageSize: number = env.pageSizeOptions[0];
    public productionItemsPageLength: number;
    public pageSizeOptions: number[] = env.pageSizeOptions;



    public selectedProductionItem: ProductionItem = new ProductionItem();
    public productionItemsLoading: boolean;
    public savingLoading: boolean;

    public productionItems: ProductionItem[] = [];

    constructor(private _api: BackendApiService, private _helper: HelperService) { }

    ngOnInit() {
        this.getProductionItems(this.productionItemsPageNumber, this.productionItemsPageSize).subscribe();
    }

    public createOrder() {
        this.savingLoading = true;
        const order: Order = {
            description: this.description,
            name: this.name,
            plannedBeginDate: this.plannedBeginDate,
            plannedEndDate: this.plannedEndDate,
            state: 'Undefined' || OrderState[OrderState[0]],
            orderQuantums: this.orderQuantums,
            orderQuantumsCount: this.orderQuantums.length
        };

        this._api.createOrder(order)
            .catch(resp => {
                this.savingLoading = false;
                alert(`Не удалось добавить заказ по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                this.orderQuantums = [];
                this.orderQuantumsDataSource = new OrderQuantumsDataSource(this.orderQuantums);
                this.selectedProductionItem = new ProductionItem();
                this.savingLoading = false;
            });
    }

    private getProductionItems(pageNumber: number, pageSize: number): Observable<ProductionItem[] | {}> {
        this.productionItemsLoading = true;
        return this._api.getProductionItems(pageNumber, pageSize)
            .do(productionItems => {
                this.productionItems = productionItems;
                this.productionItemsDataSource = new ProductionItemsDataSource(this.productionItems);
                this.updateProductionItemsPaginatorFields();
                this.productionItemsLoading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список продукции по причине: ${JSON.stringify(resp, null, 4)}`);
                this.productionItemsLoading = false;
                return Observable.empty();
            });
    }

    public isNullOrWhitespace() {
        return this._helper.isNullOrWhitespace(this.name);
    }

    public updateProductionItemsPaginatorFields() {
        this.productionItemsPageLength = (this.productionItems.length < this.productionItemsPageSize)
            ? this.productionItems.length + this.productionItemsPageNumber * this.productionItemsPageSize
            : (this.productionItemsPageNumber + 2) * this.productionItemsPageSize;
    }

    public isAlreadySelected(item: any): boolean {
        return this.orderQuantums.some(i => i.productionItem.id === item.id);
    }

    public selectProductionItem(productionItem: ProductionItem): void {
        this.selectedProductionItem = productionItem;
    }

    public isValidOrderQuatnum(): boolean {
        if (!this.selectedProductionItem.id || isNaN(+this.productionItemsCount)
            || this.productionItemsCount < 1 || isNaN(+this.itemsCountInOnePart)) {
            return false;
        }

        if (this.itemsCountInOnePart > this.productionItemsCount) {
            return false;
        }

        return true;
    }

    public addOrderQuantum() {
        const orderQuantum: OrderQuantum = {
            count: this.productionItemsCount,
            itemsCountInOnePart: this.itemsCountInOnePart,
            productionItem: this.selectedProductionItem
        };
        this.orderQuantums.push(orderQuantum);
        this.orderQuantumsDataSource = new OrderQuantumsDataSource(this.orderQuantums);
    }

    public handleProductionItemsPageEvent(event: any) {
        this.productionItemsPageNumber = event.pageIndex;
        this.productionItemsPageSize = event.pageSize;

        this.getProductionItems(this.productionItemsPageNumber, this.productionItemsPageSize).subscribe();
    }

    public deleteOrderQuantum(orderQuantum: OrderQuantum) {
        this.orderQuantums.splice(this.orderQuantums.indexOf(orderQuantum), 1);
        this.orderQuantumsDataSource = new OrderQuantumsDataSource(this.orderQuantums);
    }


}

class OrderQuantumsDataSource extends DataSource<OrderQuantum> {
    constructor(private _orderQuantums: OrderQuantum[]) {
        super();
    }

    connect(): Observable<OrderQuantum[]> {
        return Observable.of(this._orderQuantums);
    }

    disconnect() {
    }
}
