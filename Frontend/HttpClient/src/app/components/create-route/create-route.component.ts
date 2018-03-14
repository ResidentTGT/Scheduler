import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Operation, OperationType } from '../../models/operation';
import { Detail } from '../../models/detail';
import { Observable } from 'rxjs/Rx';
import { Route } from '../../models/route';
import { DetailsDataSource } from '../details/details.component';
import { environment as env } from '../../../environments/environment';
import { OperationsDataSource } from '../operations/operations.component';

@Component({
    selector: 'sch-create-route',
    templateUrl: './create-route.component.html',
    styleUrls: ['./create-route.component.scss']
})
export class CreateRouteComponent implements OnInit {

    public name: string;
    public description: string;
    public selectedDetail: Detail = null;
    public selectedOperations: Operation[] = [];
    public departmentOperations: DepartmentOperations[] = [];

    public detailsLoading: boolean;
    public operationsLoading: boolean;

    public details: Detail[] = [];
    public detailsDataSource: DetailsDataSource | null;
    public detailsDisplayedColumns = ['title', 'description'];
    public detailsPageNumber = 0;
    public detailsPageSize: number = env.pageSizeOptions[0];
    public detailsPageLength: number;

    public operations: Operation[] = [];
    public operationsDataSource: OperationsDataSource | null;
    public operationsDisplayedColumns = ['name', 'equipment', 'add-button'];
    public operationsPageNumber = 0;
    public operationsPageSize: number = env.pageSizeOptions[0];
    public operationsPageLength: number;

    public pageSizeOptions: number[] = env.pageSizeOptions;

    constructor(private _api: BackendApiService) {
    }

    ngOnInit() {
        this.getDetails(this.detailsPageNumber, this.detailsPageSize).subscribe();
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

    updateOperationsPaginatorFields() {
        this.operationsPageLength = (this.operations.length < this.operationsPageSize)
            ? this.operations.length + this.operationsPageNumber * this.operationsPageSize
            : (this.operationsPageNumber + 2) * this.operationsPageSize;
    }

    public handleDetailsPageEvent(event: any) {
        this.detailsPageNumber = event.pageIndex;
        this.detailsPageSize = event.pageSize;

        this.getDetails(this.detailsPageNumber, this.detailsPageSize).subscribe();
    }

    public handleOperationsPageEvent(event: any) {
        this.operationsPageNumber = event.pageIndex;
        this.operationsPageSize = event.pageSize;

        this.getOperationsByDetailId(+this.selectedDetail.id, this.operationsPageNumber, this.operationsPageSize).subscribe();
    }

    private getOperationsByDetailId(detailId: number, pageNumber: number, pageSize: number): Observable<Operation[] | {}> {
        this.operationsLoading = true;
        return this._api.getOperationsByDetailId(detailId, pageNumber, pageSize)
            .do(operations => {
                this.operations = operations;
                this.operationsDataSource = new OperationsDataSource(this.operations);
                this.updateOperationsPaginatorFields();
                this.operationsLoading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список операций по причине: ${JSON.stringify(resp, null, 4)}`);
                this.operationsLoading = false;
                return Observable.empty();
            });
    }

    public selectOperation(operation: Operation): void {
        this.selectedOperations.push(operation);

        let department, type;
        if (operation.equipment.workshop) {
            type = 1;
            department = operation.equipment.workshop;
        } else {
            type = 2;
            department = operation.equipment.conveyor;
        }


        if (this.departmentOperations.some(o => o.departmentId === department.id && o.departmentType === type)) {
            const departmentOperations = this.departmentOperations
                .filter(d => d.departmentId === department.id && d.departmentType === type)[0];

            if (!departmentOperations.operations.some(o => o.id === operation.id)) {
                departmentOperations.operations.push(operation);
            }
        } else {
            this.departmentOperations.push(
                {
                    departmentId: department.id,
                    departmentName: department.name,
                    departmentType: type,
                    operations: [operation]
                });
        }
    }

    public selectDetail(detail: Detail): void {
        this.selectedDetail = detail;
        this.operations = [];
        this.departmentOperations = [];
        this.getOperationsByDetailId(+this.selectedDetail.id, this.operationsPageNumber, this.operationsPageSize).subscribe();
    }

    public isOperationSelected(operation: Operation): boolean {
        return this.selectedOperations.some(o => o.id === operation.id);
    }

    public moveUpDepartment(department: DepartmentOperations): void {
        const index = this.departmentOperations.indexOf(department);

        if (index !== 0) {
            const a = this.departmentOperations[index - 1];
            const b = this.departmentOperations[index];
            this.departmentOperations[index] = a;
            this.departmentOperations[index - 1] = b;
        }
    }

    public moveDownDepartment(department: DepartmentOperations): void {
        const index = this.departmentOperations.indexOf(department);

        if (index !== this.departmentOperations.length - 1) {
            const a = this.departmentOperations[index];
            const b = this.departmentOperations[index + 1];
            this.departmentOperations[index] = b;
            this.departmentOperations[index + 1] = a;
        }
    }

    public deleteDepartment(department: DepartmentOperations): void {
        department.operations.forEach(o =>
            this.selectedOperations
                .splice(this.selectedOperations.indexOf(this.selectedOperations.filter(oper => oper.id === o.id)[0]), 1));


        this.departmentOperations.splice(this.departmentOperations
            .indexOf(this.departmentOperations
                .filter(d => d.departmentId === department.departmentId && d.departmentType === department.departmentType)[0]), 1);
    }


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

class DepartmentOperations {
    public departmentId: number;
    public departmentName: string;
    // 0 - workshop, 1 - conveyor
    public departmentType: number;
    public operations: Operation[] = [];
}
