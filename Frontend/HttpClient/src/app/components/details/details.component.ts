import { Component, OnInit } from '@angular/core';
import { Detail } from '../../models/detail';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
import { DataSource } from '@angular/cdk/collections';
import { environment as env } from '../../../environments/environment';
import { HelperService } from '../../services/helper.service';

@Component({
    selector: 'sch-details',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {

    public details: Detail[] = [];
    public dataSource: DetailsDataSource | null;
    public displayedColumns = ['title', 'description', 'cost', 'is-purchased', 'routeId'];

    public pageSizeOptions: number[] = env.pageSizeOptions;
    public pageNumber = 0;
    public pageSize: number = env.pageSizeOptions[0];
    public pageLength: number;

    public loading: boolean;

    isPurchased: Boolean = false;
    title: '';
    description: '';
    cost: number = null;

    constructor(private _api: BackendApiService, private _helper: HelperService) {
    }

    ngOnInit() {
        this.getDetails(this.pageNumber, this.pageSize);
    }

    private getDetails(pageNumber: number, pageSize: number) {
        this.loading = true;
        this._api.getDetails(pageNumber, pageSize)
            .do(details => {
                this.details = details;
                this.dataSource = new DetailsDataSource(this.details);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список деталей по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            })
            .subscribe();
    }

    public createDetail() {
        const detail: Detail = {
            title: this.title,
            description: this.description,
            cost: this.cost,
            isPurchased: this.isPurchased,
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

    public deleteDetail(detail: Detail) {
        this._api.deleteDetail(detail.id)
            .catch(resp => {
                alert(`Не удалось удалить деталь по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(_ => this.details.splice(this.details.indexOf(detail), 1));
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getDetails(this.pageNumber, this.pageSize);

    }

    updatePaginatorFields() {
        this.pageLength = (this.details.length < this.pageSize)
            ? this.details.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
    }

    public isNullOrWhitespace() {
        return this._helper.isNullOrWhitespace(this.title);
    }

}

export class DetailsDataSource extends DataSource<Detail> {
    constructor(private _details: Detail[]) {
        super();
    }

    connect(): Observable<Detail[]> {
        return Observable.of(this._details);
    }

    disconnect() {
    }
}


