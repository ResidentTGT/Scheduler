import { Pipe, PipeTransform } from '@angular/core';
import { EquipmentType } from '../models/equipment';

@Pipe({
    name: 'equipmentType'
})
export class EquipmentTypePipe implements PipeTransform {

    transform(type: any): string {
        switch (type) {
            case 'Transport' || EquipmentType.Transport: {
                type = 'Транспорт';
                break;
            }
            case 'MachiningTool' || EquipmentType.MachiningTool: {
                type = 'Станок';
                break;
            }
            case 'Undefined' || EquipmentType.Undefined: {
                type = 'Не определено';
                break;
            }
            case 'AssemblyWorkplace' || EquipmentType.AssemblyWorkplace: {
                type = 'Сборочное место';
                break;
            }
        }
        return type;
    }

}
