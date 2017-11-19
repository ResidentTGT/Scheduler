import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Operation } from '../../models/operation';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Rx';
import { CreateOperationComponent } from '../create-operation/create-operation.component';

@Component({
    selector: 'sch-operations',
    templateUrl: './operations.component.html',
    styleUrls: ['./operations.component.scss']
})
export class OperationsComponent implements OnInit {

    public operations: Operation[] = [];

    constructor(private _api: BackendApiService, private dialog: MatDialog) { }

    ngOnInit() {
        this.getOperations();
    }

    private getOperations() {
        this._api.getOperations()
            .do(operations => this.operations = operations)
            .catch(resp => {
                alert(`Не удалось загрузить список операций по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public deleteOperation(operation: Operation) {
        this._api.deleteOperation(operation.id)
            .catch(resp => {
                alert(`Не удалось удалить операцию по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(_ => this.operations.splice(this.operations.indexOf(operation), 1));
    }

    public openDialog() {
        this.dialog.open(CreateOperationComponent, { data: { operations: this.operations } });
    }
}
