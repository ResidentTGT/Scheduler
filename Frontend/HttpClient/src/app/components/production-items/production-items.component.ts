import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material';
import { CreateProductionItemComponent } from '../create-production-item/create-production-item.component';
import { ProductionItem } from '../../models/production-item';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { environment as env } from '../../../environments/environment';
import { DataSource } from '@angular/cdk/collections';
import { Router } from '@angular/router';

@Component({
    selector: 'sch-production-items',
    templateUrl: './production-items.component.html',
    styleUrls: ['./production-items.component.scss']
})
export class ProductionItemsComponent implements OnInit {

    public productionItems: ProductionItem[] = [];
    public dataSource: ProductionItemsDataSource | null;
    public displayedColumns = ['title', 'description', 'details-count', 'сhildren-count', 'oneItemIncome', 'openButton', 'deleteButton'];

    public pageSizeOptions: number[] = env.pageSizeOptions;
    public pageNumber = 0;
    public pageSize: number = env.pageSizeOptions[0];
    public pageLength: number;

    public loading: boolean;

    constructor(private _api: BackendApiService, private _router: Router) { }

    ngOnInit() {
        this.getProductionItems(this.pageNumber, this.pageSize).subscribe();
    }

    private getProductionItems(pageNumber: number, pageSize: number): Observable<ProductionItem[] | {}> {
        this.loading = true;
        return this._api.getProductionItems(pageNumber, pageSize)
            .do(productionItems => {
                this.productionItems = productionItems;
                this.dataSource = new ProductionItemsDataSource(this.productionItems);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список продукции по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            });
    }

    public deleteProductionItem(productionItem: ProductionItem) {
        this._api.deleteProductionItem(productionItem.id)
            .catch(resp => {
                alert(`Не удалось удалить продукцию по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getProductionItems(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getProductionItems(this.pageNumber, this.pageSize).subscribe();

    }

    updatePaginatorFields() {
        this.pageLength = (this.productionItems.length < this.pageSize)
            ? this.productionItems.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
    }

    public createProductionItem(): void {
        this._router.navigateByUrl('production-items/create');
    }

}

export class ProductionItemsDataSource extends DataSource<ProductionItem> {
    constructor(private _productionItems: ProductionItem[]) {
        super();
    }

    connect(): Observable<ProductionItem[]> {
        return Observable.of(this._productionItems);
    }

    disconnect() {
    }
}
