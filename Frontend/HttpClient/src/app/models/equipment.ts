export class Equipment {
    public id?: number | {};
    public name: string;
    public description: string;
    public type: EquipmentType;
}

export enum EquipmentType {
    Undefined = 0,
    MachiningTool = 1,
    AssemblyWorkplace = 2,
    Transport = 3
}
