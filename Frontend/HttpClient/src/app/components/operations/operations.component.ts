import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Operation } from '../../models/operation';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Rx';
import { CreateOperationComponent } from '../create-operation/create-operation.component';
import { environment as env } from '../../../environments/environment';
import { DataSource } from '@angular/cdk/collections';
import { Router } from '@angular/router';

@Component({
    selector: 'sch-operations',
    templateUrl: './operations.component.html',
    styleUrls: ['./operations.component.scss']
})
export class OperationsComponent implements OnInit {

    public operations: Operation[] = [];
    public dataSource: OperationsDataSource | null;
    public displayedColumns = ['name', 'description', 'main-time', 'additional-time', 'operation-type', 'equipment', 'detail', 'deleteButton'];

    public pageSizeOptions: number[] = env.pageSizeOptions;
    public pageNumber = 0;
    public pageSize: number = env.pageSizeOptions[0];
    public pageLength: number;

    public loading: boolean;

    constructor(private _api: BackendApiService, private _router: Router) { }

    ngOnInit() {
        this.getOperations(this.pageNumber, this.pageSize).subscribe();
    }

    private getOperations(pageNumber: number, pageSize: number): Observable<Operation[] | {}> {
        this.loading = true;
        return this._api.getOperations(pageNumber, pageSize)
            .do(operations => {
                this.operations = operations;
                this.dataSource = new OperationsDataSource(this.operations);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список операций по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            });
    }

    public deleteOperation(operation: Operation) {
        this._api.deleteOperation(operation.id)
            .catch(resp => {
                alert(`Не удалось удалить операцию по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getOperations(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getOperations(this.pageNumber, this.pageSize).subscribe();

    }

    updatePaginatorFields() {
        this.pageLength = (this.operations.length < this.pageSize)
            ? this.operations.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
    }

    public createOperation(): void {
        this._router.navigateByUrl('operations/create');
    }

}

export class OperationsDataSource extends DataSource<Operation> {
    constructor(private _operations: Operation[]) {
        super();
    }

    connect(): Observable<Operation[]> {
        return Observable.of(this._operations);
    }

    disconnect() {
    }
}
