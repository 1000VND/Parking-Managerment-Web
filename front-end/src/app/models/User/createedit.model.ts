export class CreateEditUserDto {
    id: number | undefined;
    userName: string = '';
    passWord: string = '';
    fullName: string = '';
    role: number | undefined;
}