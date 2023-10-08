function removeSpacesAndUnwantedChars(str)
{
    str = str.replace(/,/g, '');
    str = str.replace(/&/g, '');
    str = str.replace(/ /g, '');
    str = str.replace(/-/g, '');
    str = str.replace(/[{()}]/g, '');
    str = str.replace(/[\[\]']+/g, '');

    return str;
}