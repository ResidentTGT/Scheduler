import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Operation, OperationType } from '../../models/operation';
import { Detail } from '../../models/detail';
import { Observable } from 'rxjs/Rx';
import { Route } from '../../models/route';
import { DetailsDataSource } from '../details/details.component';
import { environment as env } from '../../../environments/environment';

@Component({
    selector: 'sch-create-route',
    templateUrl: './create-route.component.html',
    styleUrls: ['./create-route.component.scss']
})
export class CreateRouteComponent implements OnInit {

    public name: string;
    public description: string;
    public selectedDetail: Detail = null;

    public detailsLoading: boolean;

    public details: Detail[] = [];
    public detailsDataSource: DetailsDataSource | null;
    public detailsDisplayedColumns = ['title', 'description'];
    public detailsPageNumber = 0;
    public detailsPageSize: number = env.pageSizeOptions[0];
    public detailsPageLength: number;

    public pageSizeOptions: number[] = env.pageSizeOptions;

    constructor(private _api: BackendApiService) {
    }

    ngOnInit() {
        this.getDetails(this.detailsPageNumber, this.detailsPageSize).subscribe();
        //  this.getOperations();
    }

    // public createRoute() {
    //     const route: Route = {
    //         description: this.description,
    //         name: this.name,
    //         operations: this.addedOperations,
    //         detail: this.detail,
    //         operationsSequence: this.addedOperations.map(o => +o.id)
    //     };

    //     this._api.createRoute(route)
    //         .catch(resp => {
    //             alert(`Не удалось добавить маршрут по причине: ${JSON.stringify(resp.json())}`);
    //             return Observable.empty();
    //         })
    //         .subscribe(id => {
    //             route.id = id;
    //             this.routes.push(route);
    //             this.closeDialog();
    //         });
    // }

    private getDetails(pageNumber: number, pageSize: number): Observable<Detail[] | {}> {
        this.detailsLoading = true;
        return this._api.getDetails(pageNumber, pageSize)
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

    updateDetailsPaginatorFields() {
        this.detailsPageLength = (this.details.length < this.detailsPageSize)
            ? this.details.length + this.detailsPageNumber * this.detailsPageSize
            : (this.detailsPageNumber + 2) * this.detailsPageSize;
    }

    public handleDetailsPageEvent(event: any) {
        this.detailsPageNumber = event.pageIndex;
        this.detailsPageSize = event.pageSize;

        this.getDetails(this.detailsPageNumber, this.detailsPageSize).subscribe();
    }

    public selectDetail(detail: Detail) {
        this.selectedDetail = detail;
    }

    // private getOperations() {
    //     this._api.getOperations()
    //         .do(operations => this.operations = operations)
    //         .catch(resp => {
    //             alert(`Не удалось загрузить список операций по причине: ${JSON.stringify(resp.json())}`);
    //             return Observable.empty();
    //         })
    //         .subscribe();
    // }

    // public addOperation() {
    //     this.addedOperations.push(this.operation);
    //     this.viewOperations.splice(this.viewOperations.indexOf(this.operation), 1);

    //     this.operation = null;
    // }

    // public filterOperations(detail: Detail) {
    //     if (this.operations) {
    //         this.viewOperations = this.operations.filter(o => o.detail.id === detail.id);
    //     }
    // }


    // public moveUp(operation: Operation): void {
    //     const index = this.addedOperations.indexOf(operation);

    //     if (index !== 0) {
    //         const a = this.addedOperations[index - 1];
    //         const b = this.addedOperations[index];
    //         this.addedOperations[index] = a;
    //         this.addedOperations[index - 1] = b;
    //     }
    // }

    // public moveDown(operation: Operation): void {
    //     const index = this.addedOperations.indexOf(operation);

    //     if (index !== this.addedOperations.length - 1) {
    //         const a = this.addedOperations[index];
    //         const b = this.addedOperations[index + 1];
    //         this.addedOperations[index] = b;
    //         this.addedOperations[index + 1] = a;
    //     }
    // }

    // public isValidSequence(): boolean {
    //     if (this.addedOperations.some(o => OperationType[o.type.toString()] === OperationType.Machining)
    //         && this.addedOperations.filter(o => OperationType[o.type.toString()] === OperationType.Assembling).length === 1) {
    //         return true;
    //     }
    // }

}
