import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
import { Equipment, EquipmentType } from '../../models/equipment';
import { Conveyor } from '../../models/conveyor';
import { Workshop } from '../../models/workshop';

@Component({
    selector: 'sch-equipments',
    templateUrl: './equipments.component.html',
    styleUrls: ['./equipments.component.scss']
})
export class EquipmentsComponent implements OnInit {

    equipments: Equipment[] = [];

    name: '';
    description: '';
    type: string = EquipmentType[0];
    typeOptions: string[] = [];
    public workshop: Workshop;
    public conveyor: Conveyor;

    public conveyors: Conveyor[] = [];
    public workshops: Workshop[] = [];

    constructor(private _api: BackendApiService) {
    }

    ngOnInit() {
        this.getEquipments();
        this.getConveyors();
        this.getWorkshops();
        this.typeOptions = Object.keys(EquipmentType);
        this.typeOptions = this.typeOptions.slice(this.typeOptions.length / 2);
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

    public createEquipment() {
        const equipment: Equipment = {
            name: this.name,
            description: this.description,
            type: this.type || EquipmentType[this.type],
            workshop: this.workshop,
            conveyor: this.conveyor,
        };

        this._api.createEquipment(equipment)
            .catch(resp => {
                alert(`Не удалось добавить оборудование по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                equipment.id = id;
                this.equipments.push(equipment);
            });
    }

    public deleteEquipment(equipment: Equipment) {
        this._api.deleteEquipment(equipment.id)
            .catch(resp => {
                alert(`Не удалось удалить оборудование по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(_ => this.equipments.splice(this.equipments.indexOf(equipment), 1));
    }

    private getConveyors() {
        this._api.getConveyors()
            .do(conveyors => this.conveyors = conveyors)
            .catch(resp => {
                alert(`Не удалось загрузить список конвейеров по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }
    private getWorkshops() {
        this._api.getWorkshops()
            .do(workshops => this.workshops = workshops)
            .catch(resp => {
                alert(`Не удалось загрузить список цехов по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public checkInput() {
        if (this.name === '' || !this.name
            || (!this.conveyor && !this.workshop)) {
            return true;
        }
    }

}




