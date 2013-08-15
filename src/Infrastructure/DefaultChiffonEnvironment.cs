namespace Chiffon.Infrastructure
{
    using System;

    public class DefaultChiffonEnvironment : ChiffonEnvironmentBase
    {
        public DefaultChiffonEnvironment(Uri baseUri)
            : base(ChiffonCulture.Create(ChiffonLanguage.Default), baseUri) { }

        public override void Initialize() { }
    }
}