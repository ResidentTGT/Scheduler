import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProductionItem } from '../../models/production-item';
import { BackendApiService } from '../../services/backend-api.service';
import { Observable } from 'rxjs/Rx';
import { Detail } from '../../models/detail';
import { ProductionItemQuantum } from '../../models/production-item-quantum';
import { DataSource } from '@angular/cdk/collections';
import { ProductionItemsDataSource } from '../production-items/production-items.component';
import { DetailsDataSource } from '../details/details.component';
import { environment as env } from '../../../environments/environment';
import { Product } from '../../models/product';

@Component({
    selector: 'sch-create-production-item',
    templateUrl: './create-production-item.component.html',
    styleUrls: ['./create-production-item.component.scss']
})
export class CreateProductionItemComponent implements OnInit {

    public title: string;
    public description: string;
    public oneItemIncome = 0;
    public selectedItem: Product = new Product();
    public selectedItems: Product[] = [];
    public selectedItemsDataSource: ProductsDataSource | null;
    public selectedItemsDisplayedColumns = ['title', 'description', 'type', 'count', 'deleteButton'];

    public viewDetails: Boolean = true;
    public pageSizeOptions: number[] = env.pageSizeOptions;

    public productionItems: ProductionItem[] = [];
    public productionItemsDataSource: ProductionItemsDataSource | null;
    public productionItemsDisplayedColumns = ['title', 'description'];
    public productionItemsPageNumber = 0;
    public productionItemsPageSize: number = env.pageSizeOptions[0];
    public productionItemsPageLength: number;

    public details: Detail[] = [];
    public detailsDataSource: DetailsDataSource | null;
    public detailsDisplayedColumns = ['title', 'description'];
    public detailsPageNumber = 0;
    public detailsPageSize: number = env.pageSizeOptions[0];
    public detailsPageLength: number;

    public detailsLoading: boolean;
    public productionItemsLoading: boolean;
    public savingLoading: boolean;

    constructor(private _api: BackendApiService) {

    }

    ngOnInit() {
        this.getDetailsWithRoutes(this.detailsPageNumber, this.detailsPageSize).subscribe();
        this.getProductionItems(this.productionItemsPageNumber, this.productionItemsPageSize).subscribe();
    }

    private getDetailsWithRoutes(pageNumber: number, pageSize: number): Observable<Detail[] | {}> {
        this.detailsLoading = true;
        return this._api.getDetailsWithRoutes(pageNumber, pageSize)
            .do(details => {
                this.details = details;
                this.detailsDataSource = new DetailsDataSource(this.details);
                this.updateDetailsPaginatorFields();
                this.detailsLoading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список деталей по причине: ${JSON.stringify(resp, null, 4)}`);
                this.detailsLoading = false;
                return Observable.empty();
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

    updateDetailsPaginatorFields() {
        this.detailsPageLength = (this.details.length < this.detailsPageSize)
            ? this.details.length + this.detailsPageNumber * this.detailsPageSize
            : (this.detailsPageNumber + 2) * this.detailsPageSize;
    }

    updateProductionItemsPaginatorFields() {
        this.productionItemsPageLength = (this.productionItems.length < this.productionItemsPageSize)
            ? this.productionItems.length + this.productionItemsPageNumber * this.productionItemsPageSize
            : (this.productionItemsPageNumber + 2) * this.productionItemsPageSize;
    }

    public handleDetailsPageEvent(event: any) {
        this.detailsPageNumber = event.pageIndex;
        this.detailsPageSize = event.pageSize;

        this.getDetailsWithRoutes(this.detailsPageNumber, this.detailsPageSize).subscribe();
    }

    public handleProductionItemsPageEvent(event: any) {
        this.productionItemsPageNumber = event.pageIndex;
        this.productionItemsPageSize = event.pageSize;

        this.getProductionItems(this.productionItemsPageNumber, this.productionItemsPageSize).subscribe();
    }

    public selectItem(item: any, type: number): void {
        this.selectedItem.id = item.id;
        this.selectedItem.title = item.title;
        this.selectedItem.description = item.description;
        this.selectedItem.type = type;
    }

    public isAlreadySelected(item: any, type: number): boolean {
        return this.selectedItems.some(i => i.type === type && i.id === item.id);
    }

    public addProduct(): void {
        this.selectedItems.push(Object.assign(new Product(), this.selectedItem));
        this.selectedItemsDataSource = new ProductsDataSource(this.selectedItems);
    }

    public deleteItem(item: any) {
        this.selectedItems.splice(this.selectedItems.indexOf(item), 1);
        this.selectedItemsDataSource = new ProductsDataSource(this.selectedItems);
    }

    public createProductionItem() {
        this.savingLoading = true;
        const productionItem: ProductionItem = {
            title: this.title,
            description: this.description,
            addingItems: this.selectedItems,
            oneItemIncome: this.oneItemIncome
        };
        this._api.createProductionItem(productionItem)
            .catch(resp => {
                this.savingLoading = false;
                alert(`Не удалось добавить продукцию по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                this.savingLoading = false;
                window.location.reload();
            });
    }

    public isFormValid(): boolean {
        if (!this.title || !this.selectedItems.length || isNaN(this.oneItemIncome) || this.oneItemIncome < 0) {
            return false;
        }

        return true;
    }

}


class ProductsDataSource extends DataSource<Product> {
    constructor(private _products: Product[]) {
        super();
    }

    connect(): Observable<Product[]> {
        return Observable.of(this._products);
    }

    disconnect() {
    }
}



