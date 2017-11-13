import { Component, OnInit } from '@angular/core';
import { Detail } from '../../models/detail';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
@Component({
    selector: 'sch-details',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {

    details: Detail[] = [];

    isPurchased: Boolean = false;
    title: '';
    description: '';
    cost: number = null;

    constructor(private _api: BackendApiService) {
    }

    ngOnInit() {
        this.getDetails();
    }

    private getDetails() {
        this._api.getDetails()
            .do(details => this.details = details)
            .catch(resp => {
                alert(`Не удалось загрузить список деталей по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public createDetail() {
        const detail: Detail = {
            id: 0,
            title: this.title,
            description: this.description,
            cost: this.cost,
            isPurchased: this.isPurchased,
            routeId: 0
        };

        this._api.createDetail(detail)
            .catch(resp => {
                alert(`Не удалось добавить деталь по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                detail.id = id;
                this.details.push(detail);
            });
    }

}




