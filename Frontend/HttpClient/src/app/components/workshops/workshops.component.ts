import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
import { DataSource } from '@angular/cdk/collections';
import { environment as env } from '../../../environments/environment';
import { HelperService } from '../../services/helper.service';
import { Workshop } from '../../models/workshop';

@Component({
    selector: 'sch-workshops',
    templateUrl: './workshops.component.html',
    styleUrls: ['./workshops.component.scss']
})
export class WorkshopsComponent implements OnInit {

    public workshops: Workshop[] = [];
    public dataSource: WorkshopsDataSource | null;
    public displayedColumns = ['name', 'description', 'equipments-count', 'deleteButton'];

    public pageSizeOptions: number[] = env.pageSizeOptions;
    public pageNumber = 0;
    public pageSize: number = env.pageSizeOptions[0];
    public pageLength: number;

    public loading: boolean;

    public name: '';
    public description: '';

    constructor(private _api: BackendApiService, private _helper: HelperService) {
    }

    ngOnInit() {
        this.getWorkshops(this.pageNumber, this.pageSize).subscribe();
    }

    private getWorkshops(pageNumber: number, pageSize: number): Observable<Workshop[] | {}> {
        this.loading = true;
        return this._api.getWorkshops(pageNumber, pageSize)
            .do(workshops => {
                this.workshops = workshops;
                this.dataSource = new WorkshopsDataSource(this.workshops);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список цехов по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            });
    }

    public createWorkshop() {
        const workshop: Workshop = {
            name: this.name,
            description: this.description
        };
        this._api.createWorkshop(workshop)
            .catch(resp => {
                alert(`Не удалось добавить цех по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getWorkshops(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public deleteWorkshop(workshop: Workshop) {
        this._api.deleteWorkshop(workshop.id)
            .catch(resp => {
                alert(`Не удалось удалить цех по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getWorkshops(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getWorkshops(this.pageNumber, this.pageSize).subscribe();
    }

    updatePaginatorFields() {
        this.pageLength = (this.workshops.length < this.pageSize)
            ? this.workshops.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
    }

    public isNullOrWhitespace() {
        return this._helper.isNullOrWhitespace(this.name);
    }

}

export class WorkshopsDataSource extends DataSource<Workshop> {
    constructor(private _workshops: Workshop[]) {
        super();
    }

    connect(): Observable<Workshop[]> {
        return Observable.of(this._workshops);
    }

    disconnect() {
    }
}
