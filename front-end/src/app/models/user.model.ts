export class UserDto {
    id: number | undefined | null;
    userName: string = '';
    passWord: string = '';
    token: string ='';
    fullName: string = '';
    role: number | undefined;
    creationTime!: Date;
    lastModificationTime!: Date;
}
