import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material';
import { CreateProductionItemComponent } from '../create-production-item/create-production-item.component';
import { ProductionItem } from '../../models/production-item';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';

@Component({
    selector: 'sch-production-items',
    templateUrl: './production-items.component.html',
    styleUrls: ['./production-items.component.scss']
})
export class ProductionItemsComponent implements OnInit {

    public productionItems: ProductionItem[] = [];

    constructor(private _api: BackendApiService, private dialog: MatDialog) { }

    ngOnInit() {
        this.getProductionItems();
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

    public deleteProductionItem(productionItem: ProductionItem) {
        this._api.deleteProductionItem(productionItem.id)
            .catch(resp => {
                alert(`Не удалось удалить продукцию по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(_ => {

                this.productionItems.splice(this.productionItems.indexOf(productionItem), 1);
                this.productionItems
                    .filter(p => p.parentProductionItemId === productionItem.id).forEach(p => {
                        p.parentProductionItemTitle = '';
                        p.parentProductionItemId = null;
                    });
            });
    }

    public openDialog() {
        this.dialog.open(CreateProductionItemComponent, { data: { productionItems: this.productionItems } });
    }
}
