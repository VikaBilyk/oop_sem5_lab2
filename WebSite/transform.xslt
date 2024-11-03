<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/Site">
        <NewSite>
            <xsl:apply-templates select="Page"/>
        </NewSite>
    </xsl:template>

    <xsl:template match="Page">
        <NewPage>
            <NewTitle><xsl:value-of select="Title"/></NewTitle>
            <NewType><xsl:value-of select="Type"/></NewType>
            <xsl:apply-templates select="Chars"/>
            <NewAuthorize><xsl:value-of select="Authorize"/></NewAuthorize>
        </NewPage>
    </xsl:template>

    <xsl:template match="Chars">
        <NewChars>
            <NewHasEmail><xsl:value-of select="HasEmail"/></NewHasEmail>
            <NewHasNews><xsl:value-of select="HasNews"/></NewHasNews>
            <NewHasArchive><xsl:value-of select="HasArchive"/></NewHasArchive>
            <NewHasVoting><xsl:value-of select="HasVoting"/></NewHasVoting>
            <NewPaidContent><xsl:value-of select="PaidContent"/></NewPaidContent>
        </NewChars>
    </xsl:template>
</xsl:stylesheet>
