import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProductionItem } from '../../models/production-item';
import { BackendApiService } from '../../services/backend-api.service';
import { Observable } from 'rxjs/Rx';
import { Detail } from '../../models/detail';
import { ProductionItemQuantum } from '../../models/production-item-quantum';

@Component({
    selector: 'sch-create-production-item',
    templateUrl: './create-production-item.component.html',
    styleUrls: ['./create-production-item.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CreateProductionItemComponent implements OnInit {

    public title: string;
    public description: string;
    public isNode: Boolean = false;
    public productionItemId: number;
    public productionItems: ProductionItem[] = [];
    public detail: Detail;
    public detailsCount: number;
    public productionItemQuantums: ProductionItemQuantum[] = [];

    public viewDetails: Detail[] = [];

    constructor(private _api: BackendApiService
        , public matDialogRef: MatDialogRef<CreateProductionItemComponent>
        , @Inject(MAT_DIALOG_DATA) data: any) {
        this.productionItems = data.productionItems;
    }

    ngOnInit() {
        this.getDetails();
    }

    public createProductionItem() {
        const productionItem: ProductionItem = {
            title: this.title,
            description: this.description,
            isNode: this.isNode,
            parentProductionItemId: this.productionItemId,
            parentProductionItemTitle: this.productionItemId ? this.productionItems.find(p => p.id === this.productionItemId).title : null,
            productionItemQuantums: this.productionItemQuantums
        };
        this._api.createProductionItem(productionItem)
            .catch(resp => {
                alert(`Не удалось добавить продукцию по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                productionItem.id = id;
                this.productionItems.push(productionItem);
                this.closeDialog();
            });
    }

    private getDetails() {
        this._api.getDetails()
            .do(details => this.viewDetails = details)
            .catch(resp => {
                alert(`Не удалось загрузить список деталей по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public addDetail() {
        const productionItemQuantum: ProductionItemQuantum = {
            count: this.detailsCount,
            detail: this.detail
        };
       
        this.productionItemQuantums.push(productionItemQuantum);
        this.viewDetails.splice(this.viewDetails.indexOf(this.detail), 1);
        this.detail = null;
    }

    closeDialog() {
        this.matDialogRef.close();
    }
}
