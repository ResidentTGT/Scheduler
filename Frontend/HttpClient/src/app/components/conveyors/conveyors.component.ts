import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
import { DataSource } from '@angular/cdk/collections';
import { environment as env } from '../../../environments/environment';
import { HelperService } from '../../services/helper.service';
import { Conveyor } from '../../models/conveyor';

@Component({
    selector: 'sch-conveyors',
    templateUrl: './conveyors.component.html',
    styleUrls: ['./conveyors.component.scss']
})
export class ConveyorsComponent implements OnInit {
    public conveyors: Conveyor[] = [];
    public dataSource: ConveyorsDataSource | null;
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
        this.getConveyors(this.pageNumber, this.pageSize).subscribe();
    }

    private getConveyors(pageNumber: number, pageSize: number): Observable<Conveyor[] | {}> {
        this.loading = true;
        return this._api.getConveyors(pageNumber, pageSize)
            .do(conveyors => {
                this.conveyors = conveyors;
                this.dataSource = new ConveyorsDataSource(this.conveyors);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список конвейеров по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            });
    }

    public createConveyor() {
        const сonveyor: Conveyor = {
            name: this.name,
            description: this.description
        };
        this._api.createConveyor(сonveyor)
            .catch(resp => {
                alert(`Не удалось добавить конвейер по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getConveyors(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public deleteConveyor(сonveyor: Conveyor) {
        this._api.deleteConveyor(сonveyor.id)
            .catch(resp => {
                alert(`Не удалось удалить конвейер по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getConveyors(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getConveyors(this.pageNumber, this.pageSize).subscribe();
    }

    updatePaginatorFields() {
        this.pageLength = (this.conveyors.length < this.pageSize)
            ? this.conveyors.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
    }

    public isNullOrWhitespace() {
        return this._helper.isNullOrWhitespace(this.name);
    }

}

export class ConveyorsDataSource extends DataSource<Conveyor> {
    constructor(private _conveyors: Conveyor[]) {
        super();
    }

    connect(): Observable<Conveyor[]> {
        return Observable.of(this._conveyors);
    }

    disconnect() {
    }
}
