export class Equipment {
    public id?: number | {};
    public name: string;
    public description: string;
    public type: EquipmentType;

    static fromJSON(obj: any) {
        if (!obj) {
            return null;
        }

        return Object.assign(
            new Equipment(),
            obj,
            {
               type: EquipmentType[obj.type]
            }
        );
    }
}

export enum EquipmentType {
    Undefined = 0,
    MachiningTool = 1,
    AssemblyWorkplace = 2,
    Transport = 3
}