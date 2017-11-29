import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { OperationType, Operation } from '../../models/operation';
import { Equipment } from '../../models/equipment';
import { Detail } from '../../models/detail';
import { BackendApiService } from '../../services/backend-api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs/Rx';
import { Conveyor } from '../../models/conveyor';
import { Workshop } from '../../models/workshop';

@Component({
    selector: 'sch-create-operation',
    templateUrl: './create-operation.component.html',
    styleUrls: ['./create-operation.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CreateOperationComponent implements OnInit {
    public mainTime: number;
    public name: string;
    public description: string;
    public additionalTime: number;
    public type: string = OperationType[OperationType.Undefined];
    public equipment: Equipment;
    public detail: Detail;

    public operations: Operation[] = [];
    public equipments: Equipment[] = [];
    public sortedEquipments: Equipment[] = [];
    public details: Detail[] = [];
    public sortedDetails: Detail[] = [];

    public typeOptions: string[] = [];

    constructor(private _api: BackendApiService
        , public matDialogRef: MatDialogRef<CreateOperationComponent>
        , @Inject(MAT_DIALOG_DATA) data: any) {
        this.operations = data.operations;
    }

    ngOnInit() {
        this.getEquipments();
        this.getDetails();

        this.typeOptions = Object.keys(OperationType);
        this.typeOptions = this.typeOptions.slice(this.typeOptions.length / 2);
    }

    public createOperation() {
        const operation: Operation = {
            additionalTime: this.additionalTime,
            description: this.description,
            detail: this.detail,
            equipment: this.equipment,
            mainTime: this.mainTime,
            name: this.name,
            type: this.type || OperationType[this.type],
        };

        this._api.createOperation(operation)
            .catch(resp => {
                alert(`Не удалось добавить операцию по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                operation.id = id;
                this.operations.push(operation);
                this.closeDialog();
            });
    }

    private getEquipments() {
        this._api.getEquipments()
            .do(equipments => this.equipments = equipments)
            .catch(resp => {
                alert(`Не удалось загрузить список оборудования по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
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

    private changeSortedEquipments() {

        if (this.type === 'Machining') {
            this.sortedEquipments = this.equipments.filter(e => e.workshop != null);
        }
        if (this.type === 'Assembling') {
            this.sortedEquipments = this.equipments.filter(e => e.conveyor != null);
        }
        if (this.type === 'Transport') {
            this.sortedEquipments = this.equipments.filter(e => e.conveyor == null && e.workshop == null);
        }
    }

    closeDialog() {
        this.matDialogRef.close();
    }

    checkInput() {
        if (!this.name || !this.equipment || !this.detail || !this.mainTime) {
            return true;
        }
    }

}
