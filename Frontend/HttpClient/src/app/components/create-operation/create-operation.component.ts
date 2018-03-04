import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { OperationType, Operation } from '../../models/operation';
import { Equipment } from '../../models/equipment';
import { Detail } from '../../models/detail';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs/Rx';
import { Conveyor } from '../../models/conveyor';
import { Workshop } from '../../models/workshop';
import { DataSource } from '@angular/cdk/collections';
import { environment as env } from '../../../environments/environment';
import { HelperService } from '../../services/helper.service';

@Component({
    selector: 'sch-create-operation',
    templateUrl: './create-operation.component.html',
    styleUrls: ['./create-operation.component.scss'],
})
export class CreateOperationComponent implements OnInit {
    public mainTime: number;
    public name: string;
    public description: string;
    public additionalTime: number;
    public operationType: string = OperationType[OperationType.Undefined];
    public selectedEquipment: Equipment = null;
    public selectedDetail: Detail = null;

    public equipments: Equipment[] = [];
    public equipmentsDataSource: EquipmentsDataSource | null;
    public equipmentsDisplayedColumns = ['name', 'description', 'system'];
    public equipmentsPageNumber = 0;
    public equipmentsPageSize: number = env.pageSizeOptions[0];
    public equipmentsPageLength: number;
    public equipmentsLoading: boolean;

    public details: Detail[] = [];
    public detailsDataSource: DetailsDataSource | null;
    public detailsDisplayedColumns = ['title', 'description'];
    public detailsPageNumber = 0;
    public detailsPageSize: number = env.pageSizeOptions[0];
    public detailsPageLength: number;
    public detailsLoading: boolean;

    public typeOptions: string[] = [];
    public pageSizeOptions: number[] = env.pageSizeOptions;

    constructor(private _api: BackendApiService, private _helper: HelperService) {
    }

    ngOnInit() {
        this.typeOptions = Object.keys(OperationType);
        this.typeOptions = this.typeOptions.slice(this.typeOptions.length / 2);

        this.getEquipments(this.equipmentsPageNumber, this.equipmentsPageSize, OperationType[this.operationType]).subscribe();
        this.getDetails(this.detailsPageNumber, this.detailsPageSize).subscribe();
    }

    private getEquipments(pageNumber: number, pageSize: number, operationType: OperationType): Observable<Equipment[] | {}> {
        this.equipmentsLoading = true;
        return this._api.getEquipments(pageNumber, pageSize, operationType)
            .do(equipments => {
                this.equipments = equipments;
                this.equipmentsDataSource = new EquipmentsDataSource(this.equipments);
                this.updateEquipmentsPaginatorFields();
                this.equipmentsLoading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список оборудования по причине: ${JSON.stringify(resp, null, 4)}`);
                this.equipmentsLoading = false;
                return Observable.empty();
            });
    }

    private updateEquipmentsPaginatorFields() {
        this.equipmentsPageLength = (this.equipments.length < this.equipmentsPageSize)
            ? this.equipments.length + this.equipmentsPageNumber * this.equipmentsPageSize
            : (this.equipmentsPageNumber + 2) * this.equipmentsPageSize;
    }

    public updateDataAfterSelect() {
        this.selectedEquipment = null;
        this.selectedDetail = null;
        this.updateEquipmentsTable();
        this.updateDetailsTable();
    }

    private updateDetailsPaginatorFields() {
        this.detailsPageLength = (this.details.length < this.detailsPageSize)
            ? this.details.length + this.detailsPageNumber * this.detailsPageSize
            : (this.detailsPageNumber + 2) * this.detailsPageSize;
    }

    public selectEquipment(equipment: Equipment) {
        this.selectedEquipment = equipment;

        this.getDetails(this.detailsPageNumber, this.detailsPageSize, parseInt(this.selectedEquipment.id.toString(), 10))
            .subscribe(_ => {
                if (this.selectedDetail && !this.details.some(d => d.id === this.selectedDetail.id)) {
                    this.selectedDetail = null;
                }
            });
    }

    public selectDetail(detail: Detail) {
        this.selectedDetail = detail;
    }

    public handleEquipmentsPageEvent(event: any) {
        this.equipmentsPageNumber = event.pageIndex;
        this.equipmentsPageSize = event.pageSize;

        this.getEquipments(this.equipmentsPageNumber, this.equipmentsPageSize, OperationType[this.operationType]).subscribe();
    }

    public handleDetailsPageEvent(event: any) {
        this.detailsPageNumber = event.pageIndex;
        this.detailsPageSize = event.pageSize;

        this.getDetails(this.detailsPageNumber, this.detailsPageSize, parseInt(this.selectedEquipment.id.toString(), 10)).subscribe();
    }

    public updateEquipmentsTable() {
        this.equipmentsPageNumber = 0;

        this.getEquipments(this.equipmentsPageNumber, this.equipmentsPageSize, OperationType[this.operationType]).subscribe();
    }

    public updateDetailsTable() {
        this.detailsPageNumber = 0;

        this.getDetails(this.detailsPageNumber, this.detailsPageSize).subscribe();
    }

    private getDetails(pageNumber: number, pageSize: number, equipmentId: number = 0): Observable<Detail[] | {}> {

        this.detailsLoading = true;
        return this._api.getDetails(pageNumber, pageSize, equipmentId)
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

    public createOperation() {
        const operation: Operation = {
            additionalTime: this.additionalTime,
            description: this.description,
            detail: this.selectedDetail,
            equipment: this.selectedEquipment,
            mainTime: this.mainTime,
            name: this.name,
            type: this.operationType || OperationType[this.operationType],
        };

        this._api.createOperation(operation)
            .catch(resp => {
                alert(`Не удалось добавить операцию по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .subscribe(id => this.clearForm());
    }

    private clearForm(): void {
        this.mainTime = null;
        this.name = '';
        this.description = '';
        this.additionalTime = null;
        this.operationType = OperationType[OperationType.Undefined];
        this.selectedEquipment = null;
        this.selectedDetail = null;
    }


    public isFormValid(): boolean {
        if (this._helper.isNullOrWhitespace(this.name)
            || !this.selectedEquipment || !this.selectedDetail || !this.mainTime) {
            return false;
        } else {
            return true;
        }
    }


}

export class EquipmentsDataSource extends DataSource<Equipment> {
    constructor(private _equipments: Equipment[]) {
        super();
    }

    connect(): Observable<Equipment[]> {
        return Observable.of(this._equipments);
    }

    disconnect() {
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
