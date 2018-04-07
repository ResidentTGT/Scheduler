import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
import { Equipment, EquipmentType } from '../../models/equipment';
import { Conveyor } from '../../models/conveyor';
import { Workshop } from '../../models/workshop';
import { environment as env } from '../../../environments/environment';
import { DataSource } from '@angular/cdk/collections';
import { HelperService } from '../../services/helper.service';

@Component({
    selector: 'sch-equipments',
    templateUrl: './equipments.component.html',
    styleUrls: ['./equipments.component.scss']
})
export class EquipmentsComponent implements OnInit {

    equipments: Equipment[] = [];
    public dataSource: EquipmentsDataSource | null;
    public displayedColumns =
        ['name', 'description', 'type', 'system', 'cost', 'maintenanceCost', 'usingTimeResource', 'loadFactor', 'deleteButton'];

    public pageSizeOptions: number[] = env.pageSizeOptions;
    public pageNumber = 0;
    public pageSize: number = env.pageSizeOptions[0];
    public pageLength: number;

    public loading: boolean;

    public name: '';
    public description: '';
    public type: string = EquipmentType[0];
    public typeOptions: string[] = [];
    public workshop: Workshop;
    public conveyor: Conveyor;
    public cost: number;
    public maintenanceCost = 0;
    public usingTimeResource: number;
    public loadFactor = 1;

    public conveyors: Conveyor[] = [];
    public workshops: Workshop[] = [];

    constructor(private _api: BackendApiService, private _helper: HelperService) {
    }

    ngOnInit() {
        this.getEquipments(this.pageNumber, this.pageSize).subscribe();
        this.getConveyors();
        this.getWorkshops();
        this.typeOptions = Object.keys(EquipmentType);
        this.typeOptions = this.typeOptions.slice(this.typeOptions.length / 2);
    }

    private getEquipments(pageNumber: number, pageSize: number): Observable<Equipment[] | {}> {
        this.loading = true;
        return this._api.getEquipments(pageNumber, pageSize)
            .do(equipments => {
                this.equipments = equipments;
                this.dataSource = new EquipmentsDataSource(this.equipments);
                this.updatePaginatorFields();
                this.loading = false;
            })
            .catch(resp => {
                alert(`Не удалось загрузить список оборудования по причине: ${JSON.stringify(resp, null, 4)}`);
                this.loading = false;
                return Observable.empty();
            });
    }

    public createEquipment(): void {
        const equipment: Equipment = {
            name: this.name,
            description: this.description,
            type: this.type || EquipmentType[this.type],
            workshop: this.workshop,
            conveyor: this.conveyor,
            cost: this.cost,
            loadFactor: this.loadFactor,
            maintenanceCost: this.maintenanceCost,
            usingTimeResource: this.usingTimeResource
        };

        this._api.createEquipment(equipment)
            .catch(resp => {
                alert(`Не удалось добавить оборудование по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getEquipments(this.pageNumber, this.pageSize))
            .subscribe();
    }

    public deleteEquipment(equipment: Equipment) {
        this._api.deleteEquipment(equipment.id)
            .catch(resp => {
                alert(`Не удалось удалить оборудование по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .switchMap(_ => this.getEquipments(this.pageNumber, this.pageSize))
            .subscribe();
    }

    private getConveyors() {
        this._api.getConveyors()
            .do(conveyors => this.conveyors = conveyors)
            .catch(resp => {
                alert(`Не удалось загрузить список конвейеров по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .subscribe();
    }
    private getWorkshops() {
        this._api.getWorkshops()
            .do(workshops => this.workshops = workshops)
            .catch(resp => {
                alert(`Не удалось загрузить список цехов по причине: ${JSON.stringify(resp, null, 4)}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public isFormValid(): boolean {
        if (this._helper.isNullOrWhitespace(this.name)
            || ((!this.conveyor && !this.workshop) && this.type !== 'Transport')
            || isNaN(this.cost) || this.cost <= 0 || isNaN(this.maintenanceCost) || this.maintenanceCost < 0
            || isNaN(this.loadFactor) || this.loadFactor <= 0 || this.loadFactor > 1
            || isNaN(this.usingTimeResource) || this.usingTimeResource <= 0) {
            return false;
        } else {
            return true;
        }
    }

    public handlePageEvent(event: any) {
        this.pageNumber = event.pageIndex;
        this.pageSize = event.pageSize;

        this.getEquipments(this.pageNumber, this.pageSize).subscribe();
    }

    updatePaginatorFields() {
        this.pageLength = (this.equipments.length < this.pageSize)
            ? this.equipments.length + this.pageNumber * this.pageSize
            : (this.pageNumber + 2) * this.pageSize;
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




