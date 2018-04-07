import { Equipment } from './equipment';
import { Detail } from './detail';


export class Operation {
    public id?: number | {};
    public mainTime: number;
    public name: string;
    public description: string;
    public additionalTime: number;
    public type: OperationType = OperationType.Undefined;
    public equipment: Equipment;
    public detail?: Detail;
    public riggingCost: number;
    public riggingStorageCost: number;

    static fromJSON(obj: any) {
        if (!obj) {
            return null;
        }

        return Object.assign(
            new Operation(),
            obj,
            {
                type: OperationType[obj.type],
                equipment: Equipment.fromJSON(obj.equipment)
            }
        );
    }
}

export enum OperationType {
    Undefined = 0,
    Machining = 1,
    Assembling = 2,
    Transport = 3
}
