export class CarOutDto {
    id!: number;
    creationTime!: Date;
    lastModificationTime!: Date;
    isDelete!: boolean;
    licensePlateIn!: string;
    licensePlateOut!: string;
    carTimeIn!: Date;
    carTimeOut!: Date;
    imgCarIn!: string;
    imgCarOut!: string;
    typeCard!: number;
    isCarParking!: boolean;
    cost!: number;
}