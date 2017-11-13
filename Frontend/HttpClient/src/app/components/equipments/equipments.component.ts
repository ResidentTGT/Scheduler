import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
import { Equipment, EquipmentType } from '../../models/equipment';

@Component({
    selector: 'sch-equipments',
    templateUrl: './equipments.component.html',
    styleUrls: ['./equipments.component.scss']
})
export class EquipmentsComponent implements OnInit {

    equipments: Equipment[] = [];

    name: '';
    description: '';
    type: EquipmentType = EquipmentType.Undefined;
    typeOptions: string[] = [];

    constructor(private _api: BackendApiService) {
    }

    ngOnInit() {
        this.getEquipments();
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
        const equipment: any = {
            name: this.name,
            description: this.description,
            type: this.type === EquipmentType.Undefined ? EquipmentType[this.type] : this.type
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

}




