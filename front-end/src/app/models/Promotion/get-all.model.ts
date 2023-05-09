export class GetAllDataPromotionDto{
    id!: number;
    creationTime!: Date;
    lastModificationTime!: Date;
    isDelete!: boolean;

    promotionName!: string;
    fromDate!: Date;
    toDate!: Date;
    discount!: number;
    point!: number;
}